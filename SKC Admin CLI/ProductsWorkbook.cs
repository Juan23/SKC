using System.Globalization;
using ClosedXML.Excel;

namespace SKC_Admin_CLI
{
    // One catalog row whose editable fields differ from the server. Price and classification
    // are separate endpoints server-side (price is owner-gated, classification office-gated),
    // so the flags track which of the two pushes this row needs.
    public class ProductChange
    {
        public AdminProduct Server { get; set; } = null!;
        public decimal NewPrice { get; set; }
        public string NewCategory { get; set; } = string.Empty;
        public string? NewUom { get; set; }
        public decimal NewPackMultiplier { get; set; }
        public bool PriceChanged { get; set; }
        public bool ClassificationChanged { get; set; }
    }

    public class ProductsParseResult
    {
        public List<ProductChange> Changes { get; } = new();
        public int UnchangedCount { get; set; }
        public List<string> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
    }

    // Reads/writes the owner-editable products workbook: one flat "Products" sheet, one row per
    // catalog item, matched back to the server BY SKU. Editable columns are Category / UoM /
    // Pack Multiplier / Price; Brand, Base Name and Current Stock are identification only.
    // Import is a diff: only rows whose editable values differ from the live catalog get pushed.
    public static class ProductsWorkbook
    {
        private const string ProductsSheet = "Products";
        private static readonly string[] Headers =
            { "SKU", "Brand", "Base Name", "Category", "UoM", "Pack Multiplier", "Price", "Current Stock" };
        // Columns the import actually reads back (1-based): Category, UoM, Pack Multiplier, Price.
        private static readonly int[] EditableColumns = { 4, 5, 6, 7 };

        private static readonly string[] ValidCategories =
            { "RawMaterial", "BakedGood", "DecoratedGood", "Miscellaneous" };

        public static void WriteTemplate(string path, List<AdminProduct> catalog)
        {
            using var wb = new XLWorkbook();

            var ws = wb.AddWorksheet(ProductsSheet);
            for (int i = 0; i < Headers.Length; i++)
                ws.Cell(1, i + 1).Value = Headers[i];
            ws.Row(1).Style.Font.SetBold();
            foreach (int col in EditableColumns)
                ws.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGreen;
            ws.SheetView.FreezeRows(1);

            int row = 2;
            foreach (var p in catalog.OrderBy(p => p.SKU, StringComparer.OrdinalIgnoreCase))
            {
                ws.Cell(row, 1).Value = p.SKU;
                ws.Cell(row, 2).Value = p.Brand;
                ws.Cell(row, 3).Value = p.BaseName;
                ws.Cell(row, 4).Value = p.Category;
                ws.Cell(row, 5).Value = p.Uom ?? "";
                ws.Cell(row, 6).Value = p.PackMultiplier;
                ws.Cell(row, 7).Value = p.Price;
                ws.Cell(row, 8).Value = p.CurrentStock;
                row++;
            }
            ws.Columns(1, Headers.Length).AdjustToContents();

            var help = wb.AddWorksheet("How To");
            string[] lines =
            {
                "How the Products sheet works:",
                "",
                "- One row per catalog item. Rows are matched back to the server BY SKU -",
                "  don't edit the SKU column.",
                "- Only the GREEN columns are editable: Category, UoM, Pack Multiplier, Price.",
                "  Brand / Base Name / Current Stock are shown for identification only; edits",
                "  to them are ignored (stock moves on its own, so it isn't compared at all).",
                "- Category must be RawMaterial, BakedGood, DecoratedGood, or Miscellaneous.",
                "- UoM is free text (e.g. 'Sack (25kg)'); leave it blank to clear it.",
                "- Pack Multiplier is the base units per pack (e.g. 25000 for a 25kg sack in",
                "  grams); it must be a number greater than zero. Use 1 for unpacked items.",
                "- Price is the selling price; 0 means NOT sellable (hidden from every POS).",
                "- Import only pushes rows that actually differ from the live catalog, so",
                "  re-importing the same file is harmless.",
                "- Rows you delete from this file are simply left untouched on the server.",
                "- Import with:  skcadmin products import <this-file.xlsx>",
                "  Add --dry-run to preview without changing anything.",
            };
            for (int i = 0; i < lines.Length; i++)
                help.Cell(i + 1, 1).Value = lines[i];

            wb.SaveAs(path);
        }

        public static ProductsParseResult Parse(string path, List<AdminProduct> catalog)
        {
            var result = new ProductsParseResult();
            // GroupBy-first instead of ToDictionary: a (theoretical) pair of SKUs differing only
            // by case must not crash the whole import.
            var skuLookup = catalog
                .GroupBy(p => p.SKU, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            using var wb = new XLWorkbook(path);
            if (!wb.TryGetWorksheet(ProductsSheet, out var ws))
            {
                result.Errors.Add($"Workbook has no '{ProductsSheet}' sheet.");
                return result;
            }

            var seenSkus = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
            for (int row = 2; row <= lastRow; row++)
            {
                string sku = Text(ws.Cell(row, 1));
                string brand = Text(ws.Cell(row, 2));
                string baseName = Text(ws.Cell(row, 3));
                string category = Text(ws.Cell(row, 4));
                string uom = Text(ws.Cell(row, 5));
                string packText = Text(ws.Cell(row, 6));
                string priceText = Text(ws.Cell(row, 7));

                if (sku == "" && brand == "" && baseName == "" && category == ""
                    && uom == "" && packText == "" && priceText == "")
                    continue; // fully blank row - rows are independent here, no grouping semantics

                if (sku == "")
                {
                    result.Errors.Add($"Row {row}: SKU is blank but the row has data - SKU is the match key.");
                    continue;
                }
                if (!skuLookup.TryGetValue(sku, out var server))
                {
                    // The catalog is GET /api/inventory, which returns ACTIVE items only - so a
                    // deactivated SKU lands here too. Say so: "add it from the office app" is the
                    // wrong remedy for a product that already exists but was retired.
                    result.Errors.Add($"Row {row}: SKU '{sku}' is not in the active catalog - either it "
                        + "was deactivated, or it doesn't exist. This workbook edits existing products "
                        + "only; new items are added from the office app.");
                    continue;
                }
                if (!seenSkus.Add(server.SKU))
                {
                    result.Errors.Add($"Row {row}: SKU '{server.SKU}' appears more than once in the file.");
                    continue;
                }

                if (!string.Equals(brand, server.Brand, StringComparison.Ordinal)
                    || !string.Equals(baseName, server.BaseName, StringComparison.Ordinal))
                    result.Warnings.Add($"Row {row}: Brand/Base Name for '{server.SKU}' differs from the "
                        + "catalog - names aren't editable from this workbook, so that edit is ignored.");

                string? canonicalCategory = ValidCategories.FirstOrDefault(c =>
                    string.Equals(c, category, StringComparison.OrdinalIgnoreCase));
                if (canonicalCategory == null)
                {
                    result.Errors.Add($"Row {row}: Category must be RawMaterial, BakedGood, DecoratedGood, "
                        + $"or Miscellaneous (got '{category}').");
                    continue;
                }

                if (!TryPositiveDecimal(ws.Cell(row, 6), out decimal packMultiplier))
                {
                    result.Errors.Add($"Row {row}: Pack Multiplier must be a number greater than zero "
                        + $"(got '{packText}'). Use 1 for unpacked items.");
                    continue;
                }

                if (!TryPrice(ws.Cell(row, 7), out decimal price))
                {
                    result.Errors.Add($"Row {row}: Price must be a number of at least 0 (got '{priceText}').");
                    continue;
                }

                // Round to the DB columns' scale (price NUMERIC(18,2), pack_multiplier NUMERIC(18,4))
                // BEFORE comparing. Postgres rounds on write, so an unrounded 145.999 would be stored
                // as 146.00 and then compare unequal on every later import - the row would show as a
                // pending change forever and re-import would never be idempotent.
                price = Math.Round(price, 2, MidpointRounding.AwayFromZero);
                packMultiplier = Math.Round(packMultiplier, 4, MidpointRounding.AwayFromZero);

                string? newUom = uom == "" ? null : uom;
                bool priceChanged = price != server.Price;
                bool classificationChanged =
                    canonicalCategory != server.Category
                    || !string.Equals(newUom ?? "", server.Uom ?? "", StringComparison.Ordinal)
                    || packMultiplier != server.PackMultiplier;

                if (priceChanged && price == 0 && server.Price > 0)
                    result.Warnings.Add($"Row {row}: '{server.SKU}' price goes to 0 - it will disappear "
                        + "from every POS.");

                if (!priceChanged && !classificationChanged)
                {
                    result.UnchangedCount++;
                    continue;
                }

                result.Changes.Add(new ProductChange
                {
                    Server = server,
                    NewPrice = price,
                    NewCategory = canonicalCategory,
                    NewUom = newUom,
                    NewPackMultiplier = packMultiplier,
                    PriceChanged = priceChanged,
                    ClassificationChanged = classificationChanged,
                });
            }

            return result;
        }

        private static string Text(IXLCell cell) => cell.GetFormattedString().Trim();

        // Numeric columns must be read from the cell's VALUE, never its formatted string: a Price
        // column formatted to 0 decimals displays "146" for a stored 145.70, and taking the display
        // text would silently push 146.00 as the real selling price. Currency/accounting formats
        // ("PHP 1,234.50") would likewise fail to parse at all. Text cells still fall back to the
        // invariant-culture string parse, so a hand-typed "1000" keeps working.
        private static bool TryNumber(IXLCell cell, out decimal value)
        {
            value = 0;
            var v = cell.Value;
            if (v.IsNumber)
            {
                try { value = (decimal)v.GetNumber(); }
                catch (OverflowException) { return false; }
                return true;
            }
            return decimal.TryParse(Text(cell), NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryPositiveDecimal(IXLCell cell, out decimal value)
            => TryNumber(cell, out value) && value > 0;

        private static bool TryPrice(IXLCell cell, out decimal value)
            => TryNumber(cell, out value) && value >= 0;
    }
}
