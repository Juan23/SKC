using System.Globalization;
using Spectre.Console;

namespace SKC_Admin_CLI
{
    // Owner-only admin CLI. Same trust model as the SKC Admin WinForms app: the server's
    // IsOwnerCaller IP gate is the security boundary, not this program. Exit codes:
    // 0 = success, 1 = validation/API failure, 2 = aborted (user said no / non-interactive
    // run without --yes).
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            try
            {
                return await Run(args);
            }
            catch (OfflineException ex)
            {
                AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                return 1;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                return 1;
            }
        }

        private static async Task<int> Run(string[] args)
        {
            // Bare launch in a real terminal opens the interactive menu; in a redirected
            // (scripted) context it prints the help instead, so automation stays deterministic.
            if (args.Length == 0)
                return Console.IsInputRedirected || Console.IsOutputRedirected
                    ? Help()
                    : await InteractiveMenu();

            switch (args[0].ToLowerInvariant())
            {
                case "help" or "--help" or "-h":
                    return Help();

                case "health":
                    return await Health();

                case "products" when args.Length > 1 && args[1].ToLowerInvariant() == "template":
                {
                    if (args.Length < 3) return Usage("products template <path.xlsx> [--force]");
                    if (!CheckFlags(args, 3, new[] { "--force" }, "products template <path.xlsx> [--force]", out int bad))
                        return bad;
                    return await ProductsTemplate(NormalizePath(args[2]), force: args.Contains("--force"));
                }

                case "products" when args.Length > 1 && args[1].ToLowerInvariant() == "import":
                {
                    if (args.Length < 3) return Usage("products import <path.xlsx> [--dry-run] [--yes]");
                    if (!CheckFlags(args, 3, new[] { "--dry-run", "--yes" },
                            "products import <path.xlsx> [--dry-run] [--yes]", out int bad))
                        return bad;
                    return await ProductsImport(NormalizePath(args[2]),
                        dryRun: args.Contains("--dry-run"),
                        yes: args.Contains("--yes"));
                }

                case "products":
                    return await Products(args.Length > 1 ? args[1] : null);

                case "recipes" when args.Length == 1 || args[1].ToLowerInvariant() is "list" or "--all":
                {
                    int flagStart = args.Length > 1 && args[1].ToLowerInvariant() == "list" ? 2 : 1;
                    if (!CheckFlags(args, flagStart, new[] { "--all" }, "recipes [list] [--all]", out int bad))
                        return bad;
                    return await RecipesList(includeInactive: args.Contains("--all"));
                }

                case "recipes" when args[1].ToLowerInvariant() == "template":
                {
                    if (args.Length < 3) return Usage("recipes template <path.xlsx> [--force]");
                    if (!CheckFlags(args, 3, new[] { "--force" }, "recipes template <path.xlsx> [--force]", out int bad))
                        return bad;
                    return await RecipesTemplate(NormalizePath(args[2]), force: args.Contains("--force"));
                }

                case "recipes" when args[1].ToLowerInvariant() == "import":
                {
                    if (args.Length < 3) return Usage("recipes import <path.xlsx> [--dry-run] [--yes]");
                    if (!CheckFlags(args, 3, new[] { "--dry-run", "--yes" },
                            "recipes import <path.xlsx> [--dry-run] [--yes]", out int bad))
                        return bad;
                    return await RecipesImport(NormalizePath(args[2]),
                        dryRun: args.Contains("--dry-run"),
                        yes: args.Contains("--yes"));
                }

                case "recipes" when args[1].ToLowerInvariant() == "deactivate":
                {
                    if (args.Length < 3 || !int.TryParse(args[2], out int id))
                        return Usage("recipes deactivate <recipe-id> [--yes]");
                    if (!CheckFlags(args, 3, new[] { "--yes" }, "recipes deactivate <recipe-id> [--yes]", out int bad))
                        return bad;
                    return await RecipesSetActive(id, active: false, yes: args.Contains("--yes"));
                }

                case "recipes" when args[1].ToLowerInvariant() == "activate":
                {
                    if (args.Length < 3 || !int.TryParse(args[2], out int id))
                        return Usage("recipes activate <recipe-id> [--yes]");
                    if (!CheckFlags(args, 3, new[] { "--yes" }, "recipes activate <recipe-id> [--yes]", out int bad))
                        return bad;
                    return await RecipesSetActive(id, active: true, yes: args.Contains("--yes"));
                }

                case "sales" when args.Length > 1 && args[1].ToLowerInvariant() == "export":
                {
                    if (args.Length < 6)
                        return Usage("sales export <branch> <start> <end> <out.csv> [--force]");
                    if (!CheckFlags(args, 6, new[] { "--force" },
                            "sales export <branch> <start> <end> <out.csv> [--force]", out int bad))
                        return bad;
                    return await SalesExport(args[2], args[3], args[4], NormalizePath(args[5]),
                        force: args.Contains("--force"));
                }

                default:
                    AnsiConsole.MarkupLine($"[red]Unknown command:[/] {Markup.Escape(string.Join(' ', args))}");
                    return Help(exitCode: 1);
            }
        }

        // Every argument past a command's positional ones must be a flag that command actually
        // knows. Matching with args.Contains(...) alone let a typo through silently: with
        // "--dryrun" mistyped and "--yes" present, an import would push for real, with neither the
        // dry run the owner asked for nor a confirmation prompt to catch it.
        private static bool CheckFlags(string[] args, int firstFlagIndex, string[] allowed,
            string usage, out int exitCode)
        {
            for (int i = firstFlagIndex; i < args.Length; i++)
                if (!allowed.Contains(args[i], StringComparer.Ordinal))
                {
                    AnsiConsole.MarkupLine($"[red]Unknown option:[/] {Markup.Escape(args[i])}");
                    exitCode = Usage(usage);
                    return false;
                }
            exitCode = 0;
            return true;
        }

        // Windows Explorer's "Copy as path" wraps the path in double quotes. Pasted into a prompt
        // those quotes survive into the string and File.Exists fails, which in the interactive
        // menu means an unexplained re-prompt loop the owner can only escape with Ctrl-C.
        private static string NormalizePath(string path) => path.Trim().Trim('"');

        // Writing a template over an existing file destroys whatever is in it - and the owner may
        // have spent an hour editing the previous export, with the menu offering the same default
        // filename for both export and import. Confirm; in a redirected run refuse unless --force.
        private static bool ConfirmOverwrite(string path, bool force)
        {
            if (force || !File.Exists(path)) return true;
            string full = Markup.Escape(Path.GetFullPath(path));
            if (Console.IsInputRedirected || Console.IsOutputRedirected)
            {
                AnsiConsole.MarkupLine(
                    $"[yellow]{full} already exists - pass --force to overwrite it.[/]");
                return false;
            }
            return AnsiConsole.Confirm(
                $"{full} already exists. Overwrite it? [red]Any edits in that file will be lost.[/]",
                defaultValue: false);
        }

        // The interactive shell: a thin dispatch layer only. Every choice calls the exact same
        // command method the verb form uses - no business logic lives here, so the headlessly
        // testable command layer stays the whole engine. Doubles as the feature map: an action
        // missing from this menu is an action the CLI doesn't have yet.
        private static async Task<int> InteractiveMenu()
        {
            AnsiConsole.MarkupLine("[bold]skcadmin[/] - SKC owner admin  [grey](Esc-less: pick 'Exit' to quit; arrow keys + Enter)[/]");
            int consecutiveMenuFailures = 0;
            while (true)
            {
                AnsiConsole.WriteLine();
                string choice;
                try
                {
                    choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[bold]What do you want to do?[/]")
                            .AddChoices(
                                "Check server connection",
                                "Browse products",
                                "Products: export workbook (edit prices/categories in Excel)",
                                "Products: import workbook",
                                "List recipes",
                                "List recipes (including deactivated)",
                                "Recipes: export workbook (edit in Excel)",
                                "Recipes: import workbook",
                                "Recipes: deactivate one",
                                "Recipes: reactivate one",
                                "Sales: export to CSV (for Excel)",
                                "Exit"));
                    consecutiveMenuFailures = 0;
                }
                catch (Exception ex)
                {
                    // A transient console hiccup shouldn't kill the session, but a terminal
                    // that can't render the menu at all shouldn't spin forever either.
                    if (++consecutiveMenuFailures >= 3)
                    {
                        AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                        return 1;
                    }
                    continue;
                }
                if (choice == "Exit") return 0;

                try
                {
                    switch (choice)
                    {
                        case "Check server connection":
                            await Health();
                            break;

                        case "Browse products":
                            string search = AnsiConsole.Prompt(
                                new TextPrompt<string>("Search text (Enter for all):").AllowEmpty());
                            await Products(string.IsNullOrWhiteSpace(search) ? null : search.Trim());
                            break;

                        case "Products: export workbook (edit prices/categories in Excel)":
                            await ProductsTemplate(AskSavePath("products.xlsx"), force: false);
                            AnsiConsole.MarkupLine("[grey]Edit it in Excel, then come back and pick 'import workbook'.[/]");
                            break;

                        case "Products: import workbook":
                            // Not a dry run: the import itself previews the changes and asks
                            // before pushing, so the confirm step is never skipped here.
                            await ProductsImport(AskExistingPath("products.xlsx"), dryRun: false, yes: false);
                            break;

                        case "List recipes":
                            await RecipesList(includeInactive: false);
                            break;

                        case "List recipes (including deactivated)":
                            await RecipesList(includeInactive: true);
                            break;

                        case "Recipes: export workbook (edit in Excel)":
                            await RecipesTemplate(AskSavePath("recipes.xlsx"), force: false);
                            AnsiConsole.MarkupLine("[grey]Edit it in Excel, then come back and pick 'import workbook'.[/]");
                            break;

                        case "Recipes: import workbook":
                            // Not a dry run: the import itself previews the changes and asks
                            // before pushing, so the confirm step is never skipped here.
                            await RecipesImport(AskExistingPath("recipes.xlsx"), dryRun: false, yes: false);
                            break;

                        case "Recipes: deactivate one":
                            await PickRecipeAndSetActive(active: false);
                            break;

                        case "Recipes: reactivate one":
                            await PickRecipeAndSetActive(active: true);
                            break;

                        case "Sales: export to CSV (for Excel)":
                            string branch = AnsiConsole.Prompt(
                                new TextPrompt<string>("Branch name [grey](case-sensitive, e.g. Yoho or Office)[/]:"));
                            string from = AnsiConsole.Prompt(
                                new TextPrompt<string>("Start date [grey](yyyy-MM-dd)[/]:")
                                    .DefaultValue(DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd")));
                            string to = AnsiConsole.Prompt(
                                new TextPrompt<string>("End date [grey](yyyy-MM-dd)[/]:")
                                    .DefaultValue(DateTime.Today.ToString("yyyy-MM-dd")));
                            await SalesExport(branch.Trim(), from.Trim(), to.Trim(),
                                AskSavePath("sales.csv"), force: false);
                            break;
                    }
                }
                catch (OfflineException ex)
                {
                    AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                }
            }
        }

        // Both path prompts normalize before doing anything with the answer: Explorer's "Copy as
        // path" pastes the path wrapped in quotes, and File.Exists on that always fails - which in
        // the import prompt used to mean an unexplained re-prompt loop with no way out but Ctrl-C.
        private static string AskSavePath(string defaultName) => NormalizePath(
            AnsiConsole.Prompt(new TextPrompt<string>("Save as:").DefaultValue(defaultName)));

        private static string AskExistingPath(string defaultName) => NormalizePath(
            AnsiConsole.Prompt(new TextPrompt<string>("File to import:")
                .DefaultValue(defaultName)
                .Validate(p => File.Exists(NormalizePath(p))
                    ? ValidationResult.Success()
                    : ValidationResult.Error("File not found - check the path."))));

        private static async Task<int> PickRecipeAndSetActive(bool active)
        {
            // Offer only the recipes the action can actually apply to.
            var all = await AdminApiClient.GetRecipesAsync(includeInactive: true);
            var candidates = all.Where(r => r.IsActive != active).ToList();
            if (candidates.Count == 0)
            {
                AnsiConsole.MarkupLine(active
                    ? "[yellow]No deactivated recipes to reactivate.[/]"
                    : "[yellow]No active recipes.[/]");
                return 0;
            }
            const string cancel = "(cancel)";
            var picked = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(active ? "Reactivate which recipe?" : "Deactivate which recipe?")
                    .PageSize(15)
                    .AddChoices(candidates
                        .OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                        .Select(r => $"{r.RecipeId}: {r.Name}")
                        .Append(cancel)));
            if (picked == cancel) return 0;
            return await RecipesSetActive(int.Parse(picked.Split(':')[0]), active, yes: false);
        }

        private static int Help(int exitCode = 0)
        {
            AnsiConsole.MarkupLine("[bold]skcadmin[/] - SKC owner admin CLI");
            AnsiConsole.MarkupLine("[grey]Run with no arguments in a terminal to get the interactive menu.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  [green]health[/]                                   ping the central server");
            AnsiConsole.MarkupLine("  [green]products[/] [grey][[search]][/]                       list the catalog (optionally filtered)");
            AnsiConsole.MarkupLine("  [green]products template[/] <path.xlsx> [grey][[--force]][/]    write the products workbook (live catalog: price, category, UoM, pack multiplier)");
            AnsiConsole.MarkupLine("  [green]products import[/] <path.xlsx> [grey][[--dry-run]] [[--yes]][/]  validate + push edited prices/classifications (match by SKU, changed rows only)");
            AnsiConsole.MarkupLine("  [green]recipes[/] [grey][[--all]][/]                           list recipes with ingredients (--all includes deactivated)");
            AnsiConsole.MarkupLine("  [green]recipes template[/] <path.xlsx> [grey][[--force]][/]     write the recipes workbook (pre-filled with current recipes + a Products reference sheet)");
            AnsiConsole.MarkupLine("  [green]recipes import[/] <path.xlsx> [grey][[--dry-run]] [[--yes]][/]   validate + push the workbook (match by name: update existing, create new)");
            AnsiConsole.MarkupLine("  [green]recipes deactivate[/] <id> [grey][[--yes]][/]          deactivate one recipe");
            AnsiConsole.MarkupLine("  [green]recipes activate[/] <id> [grey][[--yes]][/]            bring a deactivated recipe back");
            AnsiConsole.MarkupLine("  [green]sales export[/] <branch> <start> <end> <out.csv> [grey][[--force]][/]");
            AnsiConsole.MarkupLine("                                            raw line-level sales CSV for Excel (dates as yyyy-MM-dd)");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]--force overwrites an existing output file without asking.[/]");
            return exitCode;
        }

        private static int Usage(string usage)
        {
            AnsiConsole.MarkupLine($"[yellow]Usage:[/] skcadmin {Markup.Escape(usage)}");
            return 1;
        }

        private static async Task<int> Health()
        {
            bool ok = await AdminApiClient.CheckHealthAsync();
            AnsiConsole.MarkupLine(ok
                ? "[green]Connected to central server.[/]"
                : "[red]Cannot reach the central server.[/]");
            return ok ? 0 : 1;
        }

        private static async Task<int> Products(string? search)
        {
            var catalog = await AdminApiClient.GetAllProductsAsync();
            if (!string.IsNullOrWhiteSpace(search))
                catalog = catalog.Where(p =>
                    p.SKU.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || p.Display.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("SKU");
            table.AddColumn("Name");
            table.AddColumn("Category");
            table.AddColumn(new TableColumn("Price").RightAligned());
            table.AddColumn(new TableColumn("Stock").RightAligned());
            foreach (var p in catalog.OrderBy(p => p.SKU, StringComparer.OrdinalIgnoreCase))
                table.AddRow(
                    Markup.Escape(p.SKU),
                    Markup.Escape(p.Display),
                    Markup.Escape(p.Category),
                    p.Price.ToString("N2"),
                    p.CurrentStock.ToString("N0"));
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"[grey]{catalog.Count} product(s)[/]");
            return 0;
        }

        private static async Task<int> ProductsTemplate(string path, bool force)
        {
            if (!ConfirmOverwrite(path, force))
            {
                AnsiConsole.MarkupLine("[grey]Aborted - the existing file was left alone.[/]");
                return 2;
            }
            var catalog = await AdminApiClient.GetAllProductsAsync();
            ProductsWorkbook.WriteTemplate(path, catalog);
            AnsiConsole.MarkupLine(
                $"[green]Wrote[/] {Markup.Escape(Path.GetFullPath(path))} "
                + $"[grey]({catalog.Count} product(s); green columns are the editable ones)[/]");
            return 0;
        }

        private static async Task<int> ProductsImport(string path, bool dryRun, bool yes)
        {
            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]File not found:[/] {Markup.Escape(path)}");
                return 1;
            }

            var catalog = await AdminApiClient.GetAllProductsAsync();
            var parsed = ProductsWorkbook.Parse(path, catalog);

            foreach (var w in parsed.Warnings)
                AnsiConsole.MarkupLine($"[yellow]warning:[/] {Markup.Escape(w)}");
            if (parsed.Errors.Count > 0)
            {
                foreach (var e in parsed.Errors)
                    AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(e)}");
                AnsiConsole.MarkupLine($"[red]{parsed.Errors.Count} error(s) - nothing was pushed.[/]");
                return 1;
            }

            int untouched = catalog.Count - parsed.UnchangedCount - parsed.Changes.Count;
            if (parsed.Changes.Count == 0)
            {
                AnsiConsole.MarkupLine(
                    $"[green]Everything already matches the server - nothing to push.[/] "
                    + $"[grey]({parsed.UnchangedCount} row(s) unchanged; {untouched} catalog product(s) "
                    + "not in the file are left untouched.)[/]");
                return 0;
            }

            // Preview: one row per changed field, so old -> new is explicit before anything moves.
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("SKU");
            table.AddColumn("Product");
            table.AddColumn("Field");
            table.AddColumn(new TableColumn("Current").RightAligned());
            table.AddColumn(new TableColumn("New").RightAligned());
            int priceChanges = 0, classificationChanges = 0;
            foreach (var c in parsed.Changes.OrderBy(c => c.Server.SKU, StringComparer.OrdinalIgnoreCase))
            {
                if (c.PriceChanged)
                {
                    priceChanges++;
                    table.AddRow(Markup.Escape(c.Server.SKU), Markup.Escape(c.Server.Display),
                        "Price", c.Server.Price.ToString("N2"), $"[yellow]{c.NewPrice:N2}[/]");
                }
                if (c.ClassificationChanged)
                {
                    classificationChanges++;
                    if (c.NewCategory != c.Server.Category)
                        table.AddRow(Markup.Escape(c.Server.SKU), Markup.Escape(c.Server.Display),
                            "Category", Markup.Escape(c.Server.Category), $"[yellow]{Markup.Escape(c.NewCategory)}[/]");
                    if (!string.Equals(c.NewUom ?? "", c.Server.Uom ?? "", StringComparison.Ordinal))
                        table.AddRow(Markup.Escape(c.Server.SKU), Markup.Escape(c.Server.Display),
                            "UoM", Markup.Escape(c.Server.Uom ?? "(none)"),
                            $"[yellow]{Markup.Escape(c.NewUom ?? "(none)")}[/]");
                    if (c.NewPackMultiplier != c.Server.PackMultiplier)
                        table.AddRow(Markup.Escape(c.Server.SKU), Markup.Escape(c.Server.Display),
                            "Pack Multiplier", c.Server.PackMultiplier.ToString("N0"),
                            $"[yellow]{c.NewPackMultiplier:N0}[/]");
                }
            }
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine(
                $"[grey]{parsed.Changes.Count} product(s) to update ({priceChanges} price, "
                + $"{classificationChanges} classification); {parsed.UnchangedCount} unchanged; "
                + $"{untouched} catalog product(s) not in the file are left untouched.[/]");

            if (dryRun)
            {
                AnsiConsole.MarkupLine("[grey]--dry-run: nothing was pushed.[/]");
                return 0;
            }

            if (!yes)
            {
                // Also refuse when stdout is redirected: the confirm prompt would be written into
                // the redirect and the process would appear to hang waiting for a keypress.
                if (Console.IsInputRedirected || Console.IsOutputRedirected)
                {
                    AnsiConsole.MarkupLine("[yellow]Non-interactive run: pass --yes to push (or --dry-run to preview).[/]");
                    return 2;
                }
                if (!AnsiConsole.Confirm($"Push {parsed.Changes.Count} product update(s) to the server?"))
                {
                    AnsiConsole.MarkupLine("[grey]Aborted - nothing was pushed.[/]");
                    return 2;
                }
            }

            // Push per product; the two endpoints are independent, so a partial failure just
            // leaves that half showing as a remaining change on re-import (idempotent retry).
            // The summary counts failed PRODUCTS (matching the preview's grain), not failed
            // field pushes - the per-field FAILED lines above it carry the detail.
            int failedProducts = 0;
            int pushed = 0;
            var ordered = parsed.Changes.OrderBy(c => c.Server.SKU, StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var c in ordered)
            {
                var doneParts = new List<string>();
                bool productFailed = false;
                try
                {
                    if (c.ClassificationChanged)
                    {
                        try
                        {
                            await AdminApiClient.SetClassificationAsync(
                                c.Server.SKU, c.NewCategory, c.NewUom, c.NewPackMultiplier);
                            doneParts.Add("classification");
                        }
                        catch (OfflineException) { throw; }
                        catch (Exception ex)
                        {
                            productFailed = true;
                            AnsiConsole.MarkupLine(
                                $"[red]FAILED classification for {Markup.Escape(c.Server.SKU)}:[/] {Markup.Escape(ex.Message)}");
                        }
                    }
                    if (c.PriceChanged)
                    {
                        try
                        {
                            await AdminApiClient.SetPriceAsync(c.Server.SKU, c.NewPrice);
                            doneParts.Add("price");
                        }
                        catch (OfflineException) { throw; }
                        catch (Exception ex)
                        {
                            productFailed = true;
                            AnsiConsole.MarkupLine(
                                $"[red]FAILED price for {Markup.Escape(c.Server.SKU)}:[/] {Markup.Escape(ex.Message)}");
                        }
                    }
                }
                catch (OfflineException)
                {
                    // Don't let this reach Main: OfflineException's message says "Nothing was
                    // changed", which is true only before the first push. Here N products are
                    // already committed on the server, and the operator needs to know that.
                    if (doneParts.Count > 0)
                        AnsiConsole.MarkupLine(
                            $"[green]updated[/] {Markup.Escape(c.Server.SKU)} ({string.Join(" + ", doneParts)})");
                    AnsiConsole.MarkupLine(
                        $"[red]Connection lost after {pushed + (doneParts.Count > 0 ? 1 : 0)} of "
                        + $"{ordered.Count} product update(s) were pushed.[/] Re-run the import when "
                        + "you're back online - already-pushed rows will show as unchanged.");
                    return 1;
                }
                if (productFailed) failedProducts++;
                if (doneParts.Count > 0)
                {
                    pushed++;
                    AnsiConsole.MarkupLine(
                        $"[green]updated[/] {Markup.Escape(c.Server.SKU)} ({string.Join(" + ", doneParts)})");
                }
            }

            if (failedProducts > 0)
            {
                AnsiConsole.MarkupLine($"[red]{failedProducts} product(s) failed - the rest were pushed. Fix and "
                    + "re-import; already-pushed changes will show as unchanged.[/]");
                return 1;
            }
            AnsiConsole.MarkupLine("[green]Done.[/]");
            return 0;
        }

        private static async Task<int> RecipesList(bool includeInactive)
        {
            var recipes = await AdminApiClient.GetRecipesAsync(includeInactive);
            var catalog = await AdminApiClient.GetAllProductsAsync();
            var names = catalog
                .GroupBy(p => p.SKU, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Display, StringComparer.OrdinalIgnoreCase);

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Recipe");
            table.AddColumn("Kind");
            table.AddColumn("Output");
            table.AddColumn("Ingredients (per batch)");
            if (includeInactive) table.AddColumn("Active");
            foreach (var r in recipes.OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase))
            {
                string outName = names.TryGetValue(r.OutputSku, out var n) ? n : r.OutputSku;
                string lines = string.Join("\n", r.Lines.Select(l =>
                {
                    string inName = names.TryGetValue(l.InputSku, out var m) ? m : l.InputSku;
                    return $"{l.Qty:N0} x {inName} [{l.InputSku}]";
                }));
                var cells = new List<string>
                {
                    r.RecipeId.ToString(),
                    Markup.Escape(r.Name),
                    Markup.Escape(r.Kind),
                    Markup.Escape($"{r.OutputQty:N0} x {outName} [{r.OutputSku}]"),
                    Markup.Escape(lines),
                };
                if (includeInactive) cells.Add(r.IsActive ? "[green]yes[/]" : "[grey]no[/]");
                table.AddRow(cells.ToArray());
            }
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine(includeInactive
                ? $"[grey]{recipes.Count} recipe(s), {recipes.Count(r => !r.IsActive)} inactive[/]"
                : $"[grey]{recipes.Count} active recipe(s)[/]");
            return 0;
        }

        private static async Task<int> RecipesTemplate(string path, bool force)
        {
            if (!ConfirmOverwrite(path, force))
            {
                AnsiConsole.MarkupLine("[grey]Aborted - the existing file was left alone.[/]");
                return 2;
            }
            var catalogTask = AdminApiClient.GetAllProductsAsync();
            var recipesTask = AdminApiClient.GetRecipesAsync();
            var catalog = await catalogTask;
            var recipes = await recipesTask;

            RecipeWorkbook.WriteTemplate(path, catalog, recipes);
            AnsiConsole.MarkupLine(
                $"[green]Wrote[/] {Markup.Escape(Path.GetFullPath(path))} "
                + $"[grey]({recipes.Count} recipe(s), {catalog.Count} product(s) in the reference sheet)[/]");
            return 0;
        }

        private static async Task<int> RecipesImport(string path, bool dryRun, bool yes)
        {
            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]File not found:[/] {Markup.Escape(path)}");
                return 1;
            }

            var catalogTask = AdminApiClient.GetAllProductsAsync();
            // Fetch inactive recipes too. They're invisible to the name matching below, which is
            // what let a recipe deactivated after the template was exported come back as a silent
            // CREATE - and the resulting duplicate then trips this command's own ambiguity guard
            // on every later import. Their SKUs also widen the grandfathered set Parse accepts.
            var serverTask = AdminApiClient.GetRecipesAsync(includeInactive: true);
            var catalog = await catalogTask;
            var all = await serverTask;
            var server = all.Where(r => r.IsActive).ToList();

            var parsed = RecipeWorkbook.Parse(path, catalog, all);

            foreach (var w in parsed.Warnings)
                AnsiConsole.MarkupLine($"[yellow]warning:[/] {Markup.Escape(w)}");
            if (parsed.Errors.Count > 0)
            {
                foreach (var e in parsed.Errors)
                    AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(e)}");
                AnsiConsole.MarkupLine($"[red]{parsed.Errors.Count} error(s) - nothing was pushed.[/]");
                return 1;
            }
            if (parsed.Recipes.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]The workbook contains no recipes - nothing to do.[/]");
                return 0;
            }

            // Match by name (case-insensitive). Identical recipes are skipped; recipes on the
            // server that aren't in the file are left untouched. The server has no unique
            // constraint on recipe names, so two active recipes CAN share a name - refuse to
            // guess which one a file row means.
            var nameGroups = server.GroupBy(r => r.Name, StringComparer.OrdinalIgnoreCase).ToList();
            var ambiguous = nameGroups.Where(g => g.Count() > 1)
                .Where(g => parsed.Recipes.Any(r => string.Equals(r.Name, g.Key, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            if (ambiguous.Count > 0)
            {
                foreach (var g in ambiguous)
                    AnsiConsole.MarkupLine(
                        $"[red]error:[/] the server has {g.Count()} active recipes named "
                        + $"'{Markup.Escape(g.Key)}' (ids {string.Join(", ", g.Select(r => r.RecipeId))}) - "
                        + "can't tell which one this file means. Deactivate the extra one first "
                        + "([green]skcadmin recipes deactivate <id>[/]) and re-import.");
                AnsiConsole.MarkupLine("[red]Nothing was pushed.[/]");
                return 1;
            }
            var byName = nameGroups.ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            // A name that matches only a DEACTIVATED recipe would otherwise fall through to CREATE,
            // quietly producing two same-named recipes - and from then on the ambiguity check above
            // blocks every import until one is deactivated again. Refuse and name the id instead.
            var inactiveByName = all.Where(r => !r.IsActive)
                .Where(r => !byName.ContainsKey(r.Name))
                .GroupBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            var revived = parsed.Recipes
                .Where(r => inactiveByName.ContainsKey(r.Name))
                .Select(r => inactiveByName[r.Name])
                .ToList();
            if (revived.Count > 0)
            {
                foreach (var r in revived)
                    AnsiConsole.MarkupLine(
                        $"[red]error:[/] '{Markup.Escape(r.Name)}' is in this file but the server's "
                        + $"recipe of that name (id {r.RecipeId}) is deactivated. Creating it again "
                        + "would leave two recipes sharing a name. Reactivate it first "
                        + $"([green]skcadmin recipes activate {r.RecipeId}[/]) and re-import, or "
                        + "rename the one in the file.");
                AnsiConsole.MarkupLine("[red]Nothing was pushed.[/]");
                return 1;
            }

            var creates = new List<Recipe>();
            var updates = new List<(Recipe file, Recipe existing)>();
            var unchanged = new List<Recipe>();
            foreach (var r in parsed.Recipes)
            {
                if (!byName.TryGetValue(r.Name, out var existing)) creates.Add(r);
                else if (SameRecipe(r, existing)) unchanged.Add(r);
                else updates.Add((r, existing));
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Action");
            table.AddColumn("Recipe");
            table.AddColumn("Kind");
            table.AddColumn("Output");
            table.AddColumn(new TableColumn("Ingredients").RightAligned());
            foreach (var r in creates)
                table.AddRow("[green]CREATE[/]", Markup.Escape(r.Name), Markup.Escape(r.Kind),
                    Markup.Escape($"{r.OutputQty:N0} x {r.OutputSku}"), r.Lines.Count.ToString());
            foreach (var (r, existing) in updates)
                table.AddRow("[yellow]UPDATE[/]", Markup.Escape(r.Name), Markup.Escape(r.Kind),
                    Markup.Escape($"{r.OutputQty:N0} x {r.OutputSku}"), r.Lines.Count.ToString());
            foreach (var r in unchanged)
                table.AddRow("[grey]unchanged[/]", Markup.Escape(r.Name), Markup.Escape(r.Kind),
                    Markup.Escape($"{r.OutputQty:N0} x {r.OutputSku}"), r.Lines.Count.ToString());
            AnsiConsole.Write(table);

            int untouched = server.Count(s => !parsed.Recipes.Any(r =>
                string.Equals(r.Name, s.Name, StringComparison.OrdinalIgnoreCase)));
            AnsiConsole.MarkupLine(
                $"[grey]{creates.Count} to create, {updates.Count} to update, {unchanged.Count} unchanged; "
                + $"{untouched} server recipe(s) not in the file are left untouched.[/]");

            if (dryRun)
            {
                AnsiConsole.MarkupLine("[grey]--dry-run: nothing was pushed.[/]");
                return 0;
            }
            if (creates.Count == 0 && updates.Count == 0)
            {
                AnsiConsole.MarkupLine("[green]Everything already matches the server - nothing to push.[/]");
                return 0;
            }

            if (!yes)
            {
                // Also refuse when stdout is redirected: the confirm prompt would be written into
                // the redirect and the process would appear to hang waiting for a keypress.
                if (Console.IsInputRedirected || Console.IsOutputRedirected)
                {
                    AnsiConsole.MarkupLine("[yellow]Non-interactive run: pass --yes to push (or --dry-run to preview).[/]");
                    return 2;
                }
                if (!AnsiConsole.Confirm($"Push {creates.Count + updates.Count} change(s) to the server?"))
                {
                    AnsiConsole.MarkupLine("[grey]Aborted - nothing was pushed.[/]");
                    return 2;
                }
            }

            int failed = 0;
            int pushed = 0;
            int total = creates.Count + updates.Count;
            // Shared by both loops below: OfflineException must not reach Main, whose message says
            // "Nothing was changed" - true only until the first successful push.
            int ConnectionLost()
            {
                AnsiConsole.MarkupLine(
                    $"[red]Connection lost after {pushed} of {total} recipe change(s) were pushed.[/] "
                    + "Re-run the import when you're back online - already-pushed recipes will show "
                    + "as unchanged.");
                return 1;
            }
            foreach (var r in creates)
            {
                try
                {
                    await AdminApiClient.CreateRecipeAsync(r);
                    pushed++;
                    AnsiConsole.MarkupLine($"[green]created[/] {Markup.Escape(r.Name)}");
                }
                catch (OfflineException) { return ConnectionLost(); }
                catch (Exception ex)
                {
                    failed++;
                    AnsiConsole.MarkupLine($"[red]FAILED to create {Markup.Escape(r.Name)}:[/] {Markup.Escape(ex.Message)}");
                }
            }
            foreach (var (r, existing) in updates)
            {
                try
                {
                    await AdminApiClient.UpdateRecipeAsync(existing.RecipeId, r);
                    pushed++;
                    AnsiConsole.MarkupLine($"[yellow]updated[/] {Markup.Escape(r.Name)}");
                }
                catch (OfflineException) { return ConnectionLost(); }
                catch (Exception ex)
                {
                    failed++;
                    AnsiConsole.MarkupLine($"[red]FAILED to update {Markup.Escape(r.Name)}:[/] {Markup.Escape(ex.Message)}");
                }
            }

            if (failed > 0)
            {
                AnsiConsole.MarkupLine($"[red]{failed} recipe(s) failed - the rest were pushed. Fix and re-import; "
                    + "already-pushed recipes will show as unchanged.[/]");
                return 1;
            }
            AnsiConsole.MarkupLine("[green]Done.[/]");
            return 0;
        }

        // Deactivate and activate are the same operation with the flag flipped, so they share one
        // method: same lookup, same confirm/--yes/redirection handling, same already-in-that-state
        // short-circuit. Looks the recipe up across BOTH states - "no active recipe with id 7" is
        // the wrong message when id 7 exists and is simply already deactivated.
        private static async Task<int> RecipesSetActive(int id, bool active, bool yes)
        {
            string verb = active ? "activate" : "deactivate";
            var all = await AdminApiClient.GetRecipesAsync(includeInactive: true);
            var recipe = all.FirstOrDefault(r => r.RecipeId == id);
            if (recipe == null)
            {
                AnsiConsole.MarkupLine($"[red]No recipe with id {id}.[/] Run "
                    + "[green]skcadmin recipes --all[/] to see ids.");
                return 1;
            }
            if (recipe.IsActive == active)
            {
                AnsiConsole.MarkupLine($"[yellow]'{Markup.Escape(recipe.Name)}' (id {id}) is already "
                    + $"{(active ? "active" : "deactivated")} - nothing to do.[/]");
                return 0;
            }

            if (!yes)
            {
                if (Console.IsInputRedirected || Console.IsOutputRedirected)
                {
                    AnsiConsole.MarkupLine($"[yellow]Non-interactive run: pass --yes to {verb}.[/]");
                    return 2;
                }
                if (!AnsiConsole.Confirm($"{char.ToUpperInvariant(verb[0])}{verb[1..]} "
                        + $"'{Markup.Escape(recipe.Name)}' (id {id})?"))
                {
                    AnsiConsole.MarkupLine("[grey]Aborted.[/]");
                    return 2;
                }
            }

            await AdminApiClient.SetRecipeActiveAsync(id, active);
            AnsiConsole.MarkupLine($"[green]{(active ? "Activated" : "Deactivated")}[/] "
                + $"{Markup.Escape(recipe.Name)} (id {id}).");
            return 0;
        }

        // Raw line-level CSV, no aggregation - the analysis happens in Excel. One row per sale
        // line, exactly as /api/sales/lines returns it.
        private static async Task<int> SalesExport(string branch, string startText, string endText,
            string path, bool force)
        {
            if (!TryDate(startText, out DateTime start))
                return Usage($"sales export - start date '{startText}' isn't a date (use yyyy-MM-dd)");
            if (!TryDate(endText, out DateTime end))
                return Usage($"sales export - end date '{endText}' isn't a date (use yyyy-MM-dd)");
            // A bare date parses to midnight, which would exclude everything sold that day - the
            // owner means "through the end of the 20th", not "up to 00:00 on the 20th".
            if (end.TimeOfDay == TimeSpan.Zero) end = end.Date.AddDays(1).AddTicks(-1);
            if (end < start)
            {
                AnsiConsole.MarkupLine("[red]The end date is before the start date.[/]");
                return 1;
            }
            if (!ConfirmOverwrite(path, force))
            {
                AnsiConsole.MarkupLine("[grey]Aborted - the existing file was left alone.[/]");
                return 2;
            }

            var lines = await AdminApiClient.GetSalesLinesAsync(branch, start, end);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SaleNo,ClientSaleId,SoldAt,StaffName,Voided,SKU,Description,Qty,UnitPrice,LineTotal,ShortfallQty");
            foreach (var l in lines)
                sb.AppendLine(string.Join(",",
                    Csv(l.SaleNo.ToString(CultureInfo.InvariantCulture)),
                    Csv(l.ClientSaleId),
                    Csv(l.SoldAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                    Csv(l.StaffName),
                    Csv(l.Voided ? "yes" : "no"),
                    Csv(l.SKU ?? ""),
                    Csv(l.Description),
                    Csv(l.Qty.ToString(CultureInfo.InvariantCulture)),
                    Csv(l.UnitPrice.ToString(CultureInfo.InvariantCulture)),
                    Csv(l.LineTotal.ToString(CultureInfo.InvariantCulture)),
                    Csv(l.ShortfallQty.ToString(CultureInfo.InvariantCulture))));

            // UTF-8 WITH a BOM: without it Excel opens the file as the system codepage and mangles
            // any non-ASCII product name.
            await File.WriteAllTextAsync(path, sb.ToString(), new System.Text.UTF8Encoding(true));
            AnsiConsole.MarkupLine(
                $"[green]Wrote[/] {Markup.Escape(Path.GetFullPath(path))} "
                + $"[grey]({lines.Count} sale line(s) for {Markup.Escape(branch)}, "
                + $"{start:yyyy-MM-dd} to {end:yyyy-MM-dd})[/]");
            if (lines.Count == 0)
                AnsiConsole.MarkupLine("[yellow]No sales in that range - check the branch name "
                    + "(it's case-sensitive) and the dates.[/]");
            return 0;
        }

        private static bool TryDate(string text, out DateTime value)
            => DateTime.TryParse(text, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out value);

        private static string Csv(string field)
            => field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r')
                ? "\"" + field.Replace("\"", "\"\"") + "\""
                : field;

        private static bool SameRecipe(Recipe file, Recipe server)
        {
            if (!string.Equals(file.Kind, server.Kind, StringComparison.Ordinal)) return false;
            if (!string.Equals(file.OutputSku, server.OutputSku, StringComparison.OrdinalIgnoreCase)) return false;
            if (file.OutputQty != server.OutputQty) return false;
            if (file.Lines.Count != server.Lines.Count) return false;
            var serverLines = server.Lines.ToDictionary(l => l.InputSku, l => l.Qty, StringComparer.OrdinalIgnoreCase);
            return file.Lines.All(l => serverLines.TryGetValue(l.InputSku, out int qty) && qty == l.Qty);
        }
    }
}
