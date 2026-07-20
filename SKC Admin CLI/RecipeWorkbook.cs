using System.Globalization;
using ClosedXML.Excel;

namespace SKC_Admin_CLI
{
    public class ParseResult
    {
        public List<Recipe> Recipes { get; } = new();
        public List<string> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
    }

    // Reads/writes the owner-editable recipes workbook. One flat "Recipes" sheet, one row per
    // input line; the recipe header columns (Name/Kind/Output SKU/Output Qty) are filled on the
    // recipe's first row and left blank on its continuation rows. SKUs are the join key - names
    // in the Products reference sheet are informational only.
    public static class RecipeWorkbook
    {
        private const string RecipesSheet = "Recipes";
        private static readonly string[] Headers =
            { "Recipe Name", "Kind", "Output SKU", "Output Qty", "Input SKU", "Input Qty" };

        public static void WriteTemplate(string path, List<AdminProduct> catalog, List<Recipe> existing)
        {
            using var wb = new XLWorkbook();

            var ws = wb.AddWorksheet(RecipesSheet);
            for (int i = 0; i < Headers.Length; i++)
                ws.Cell(1, i + 1).Value = Headers[i];
            ws.Row(1).Style.Font.SetBold();
            ws.SheetView.FreezeRows(1);

            // Pre-fill with the live recipes so the file starts as the current truth: edit and
            // re-import. Falls back to a placeholder example when no recipes exist yet.
            int row = 2;
            foreach (var r in existing.OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase))
            {
                bool first = true;
                foreach (var line in r.Lines)
                {
                    if (first)
                    {
                        ws.Cell(row, 1).Value = r.Name;
                        ws.Cell(row, 2).Value = r.Kind;
                        ws.Cell(row, 3).Value = r.OutputSku;
                        ws.Cell(row, 4).Value = r.OutputQty;
                        first = false;
                    }
                    ws.Cell(row, 5).Value = line.InputSku;
                    ws.Cell(row, 6).Value = line.Qty;
                    row++;
                }
                if (first) // recipe with no lines shouldn't exist server-side, but stay robust
                {
                    ws.Cell(row, 1).Value = r.Name;
                    ws.Cell(row, 2).Value = r.Kind;
                    ws.Cell(row, 3).Value = r.OutputSku;
                    ws.Cell(row, 4).Value = r.OutputQty;
                    row++;
                }
            }
            if (existing.Count == 0)
            {
                ws.Cell(2, 1).Value = "EXAMPLE - Chiffon Batch (delete me)";
                ws.Cell(2, 2).Value = "Baking";
                ws.Cell(2, 3).Value = "PUT-OUTPUT-SKU-HERE";
                ws.Cell(2, 4).Value = 10;
                ws.Cell(2, 5).Value = "PUT-INPUT-SKU-HERE";
                ws.Cell(2, 6).Value = 1000;
                ws.Cell(3, 5).Value = "PUT-2ND-INPUT-SKU-HERE";
                ws.Cell(3, 6).Value = 500;
            }
            ws.Columns(1, 6).AdjustToContents();

            var prod = wb.AddWorksheet("Products");
            string[] prodHeaders = { "SKU", "Brand", "Base Name", "Category", "Price", "Current Stock" };
            for (int i = 0; i < prodHeaders.Length; i++)
                prod.Cell(1, i + 1).Value = prodHeaders[i];
            prod.Row(1).Style.Font.SetBold();
            prod.SheetView.FreezeRows(1);
            int prow = 2;
            foreach (var p in catalog.OrderBy(p => p.SKU, StringComparer.OrdinalIgnoreCase))
            {
                prod.Cell(prow, 1).Value = p.SKU;
                prod.Cell(prow, 2).Value = p.Brand;
                prod.Cell(prow, 3).Value = p.BaseName;
                prod.Cell(prow, 4).Value = p.Category;
                prod.Cell(prow, 5).Value = p.Price;
                prod.Cell(prow, 6).Value = p.CurrentStock;
                prow++;
            }
            prod.Columns(1, 6).AdjustToContents();

            var help = wb.AddWorksheet("How To");
            string[] lines =
            {
                "How the Recipes sheet works:",
                "",
                "- One row per ingredient (input line).",
                "- Fill Recipe Name / Kind / Output SKU / Output Qty only on the recipe's FIRST row;",
                "  leave those four columns blank on the rows below it - they belong to the same recipe.",
                "- A fully blank row ends a recipe. The next non-blank row must start a new recipe",
                "  (with its header columns filled).",
                "- Kind must be 'Baking' or 'Decorating'.",
                "- All SKUs must exist in the catalog (see the Products sheet). Names don't matter; SKUs do.",
                "- Quantities are whole numbers in BASE units (grams / pieces), same as everywhere else.",
                "- Recipes are matched to the server BY NAME: an existing name is updated (its ingredient",
                "  list is replaced by what's in this file), a new name is created.",
                "- Recipes on the server that are NOT in this file are left untouched.",
                "- Import with:  skcadmin recipes import <this-file.xlsx>",
                "  Add --dry-run to preview without changing anything.",
            };
            for (int i = 0; i < lines.Length; i++)
                help.Cell(i + 1, 1).Value = lines[i];

            wb.SaveAs(path);
        }

        public static ParseResult Parse(string path, List<AdminProduct> catalog, List<Recipe> serverRecipes)
        {
            var result = new ParseResult();
            // GroupBy-first instead of ToDictionary: a (theoretical) pair of SKUs differing only
            // by case must not crash the whole import.
            var skuLookup = catalog
                .GroupBy(p => p.SKU, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            // GET /api/inventory returns active items only, but existing recipes may legitimately
            // reference a since-deactivated SKU - the server keeps those lines. Accept any SKU the
            // server's current recipes already use, so a template round-trip can't block itself.
            // Maps case-insensitively to the server's exact casing: inventory.sku is a plain
            // case-sensitive FK, so pushing the cell's raw casing would fail at the constraint.
            var grandfatheredSkus = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in serverRecipes)
            {
                grandfatheredSkus[r.OutputSku] = r.OutputSku;
                foreach (var l in r.Lines) grandfatheredSkus[l.InputSku] = l.InputSku;
            }

            using var wb = new XLWorkbook(path);
            if (!wb.TryGetWorksheet(RecipesSheet, out var ws))
            {
                result.Errors.Add($"Workbook has no '{RecipesSheet}' sheet.");
                return result;
            }

            Recipe? current = null;
            bool separatedByBlankRow = false;
            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
            for (int row = 2; row <= lastRow; row++)
            {
                string name = Text(ws.Cell(row, 1));
                string kind = Text(ws.Cell(row, 2));
                string outputSku = Text(ws.Cell(row, 3));
                string outputQtyText = Text(ws.Cell(row, 4));
                string inputSku = Text(ws.Cell(row, 5));
                string inputQtyText = Text(ws.Cell(row, 6));

                bool allBlank = name == "" && kind == "" && outputSku == "" && outputQtyText == ""
                                && inputSku == "" && inputQtyText == "";
                if (allBlank)
                {
                    // A blank row ends the current recipe: an ingredient row after it must start
                    // a new recipe header, so a forgotten header can't silently fold ingredients
                    // into the previous recipe.
                    separatedByBlankRow = true;
                    continue;
                }

                if (name != "")
                {
                    // header row: starts a new recipe
                    var recipe = new Recipe { Name = name };

                    if (string.Equals(kind, "Baking", StringComparison.OrdinalIgnoreCase)) recipe.Kind = "Baking";
                    else if (string.Equals(kind, "Decorating", StringComparison.OrdinalIgnoreCase)) recipe.Kind = "Decorating";
                    else result.Errors.Add($"Row {row}: Kind must be 'Baking' or 'Decorating' (got '{kind}').");

                    if (outputSku == "")
                        result.Errors.Add($"Row {row}: Output SKU is required on a recipe's first row.");
                    else if (!skuLookup.TryGetValue(outputSku, out var outProduct))
                    {
                        if (grandfatheredSkus.TryGetValue(outputSku, out var canonicalOutput))
                        {
                            recipe.OutputSku = canonicalOutput;
                            result.Warnings.Add($"Row {row}: Output SKU '{canonicalOutput}' is deactivated in the "
                                + "catalog but an existing recipe already uses it - kept.");
                        }
                        else
                            result.Errors.Add($"Row {row}: Output SKU '{outputSku}' is not in the catalog.");
                    }
                    else
                    {
                        recipe.OutputSku = outProduct.SKU; // canonical casing
                        string expected = recipe.Kind == "Decorating" ? "DecoratedGood" : "BakedGood";
                        if (recipe.Kind != "" && outProduct.Category != expected)
                            result.Warnings.Add(
                                $"Row {row}: '{recipe.Name}' is {recipe.Kind} but its output '{outProduct.SKU}' "
                                + $"is categorized {outProduct.Category}, not {expected}.");
                    }

                    if (!TryQty(ws.Cell(row, 4), out int outputQty))
                        result.Errors.Add($"Row {row}: Output Qty must be a whole number greater than zero (got '{outputQtyText}').");
                    else recipe.OutputQty = outputQty;

                    if (result.Recipes.Any(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase)))
                        result.Errors.Add($"Row {row}: recipe name '{name}' appears more than once in the file.");

                    result.Recipes.Add(recipe);
                    current = recipe;
                    separatedByBlankRow = false;
                }
                else if (kind != "" || outputSku != "" || outputQtyText != "")
                {
                    result.Errors.Add(
                        $"Row {row}: Kind/Output columns are filled but Recipe Name is blank - "
                        + "header fields belong only on a recipe's first row.");
                    continue;
                }

                if (inputSku != "" || inputQtyText != "")
                {
                    if (current == null)
                    {
                        result.Errors.Add($"Row {row}: ingredient row appears before any recipe header row.");
                        continue;
                    }
                    if (separatedByBlankRow && name == "")
                    {
                        result.Errors.Add($"Row {row}: ingredient row after a blank row - a blank row ends "
                            + $"a recipe. Fill in the Recipe Name/Kind/Output columns here, or remove the blank "
                            + $"row if this ingredient belongs to '{current.Name}'.");
                        continue;
                    }
                    if (inputSku == "")
                    {
                        result.Errors.Add($"Row {row}: Input Qty is filled but Input SKU is blank.");
                        continue;
                    }
                    string canonicalInputSku;
                    if (skuLookup.TryGetValue(inputSku, out var inProduct))
                        canonicalInputSku = inProduct.SKU;
                    else if (grandfatheredSkus.TryGetValue(inputSku, out var canonicalGrandfathered))
                    {
                        canonicalInputSku = canonicalGrandfathered;
                        result.Warnings.Add($"Row {row}: Input SKU '{canonicalGrandfathered}' is deactivated in the "
                            + "catalog but an existing recipe already uses it - kept.");
                    }
                    else
                    {
                        result.Errors.Add($"Row {row}: Input SKU '{inputSku}' is not in the catalog.");
                        continue;
                    }
                    if (!TryQty(ws.Cell(row, 6), out int qty))
                    {
                        result.Errors.Add($"Row {row}: Input Qty must be a whole number greater than zero (got '{inputQtyText}').");
                        continue;
                    }
                    if (current.Lines.Any(l => string.Equals(l.InputSku, canonicalInputSku, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Errors.Add($"Row {row}: '{current.Name}' lists input '{canonicalInputSku}' twice - "
                            + "combine them into one row.");
                        continue;
                    }
                    current.Lines.Add(new RecipeLine { InputSku = canonicalInputSku, Qty = qty });
                }
            }

            foreach (var r in result.Recipes.Where(r => r.Lines.Count == 0))
                result.Errors.Add($"Recipe '{r.Name}' has no ingredient rows - a recipe needs at least one input line.");

            foreach (var r in result.Recipes.Where(r =>
                         r.Kind == "Decorating"
                         && !r.Lines.Any(l => skuLookup.TryGetValue(l.InputSku, out var p) && p.Category == "BakedGood")))
                result.Warnings.Add($"Recipe '{r.Name}' is Decorating but none of its inputs is a BakedGood.");

            return result;
        }

        private static string Text(IXLCell cell) => cell.GetFormattedString().Trim();

        // Read from the cell's VALUE, not its formatted string: a qty column formatted to 0 decimals
        // displays "1000" for a stored 1000.4, and taking the display text would silently accept a
        // fractional qty as a whole number. Text cells still fall back to the invariant-culture parse.
        private static bool TryQty(IXLCell cell, out int qty)
        {
            qty = 0;
            decimal d;
            var v = cell.Value;
            if (v.IsNumber)
            {
                try { d = (decimal)v.GetNumber(); }
                catch (OverflowException) { return false; }
            }
            else if (!decimal.TryParse(Text(cell), NumberStyles.Number, CultureInfo.InvariantCulture, out d))
                return false;

            if (d <= 0 || d != decimal.Truncate(d) || d > int.MaxValue) return false;
            qty = (int)d;
            return true;
        }
    }
}
