namespace SKC_Admin_CLI
{
    // Mirrors SKC Admin/Dtos.cs (minus the WinForms-only [Browsable] attribute) - the
    // no-shared-code convention: each app carries its own copy of the shapes it consumes.
    public class AdminProduct
    {
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BaseName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = "RawMaterial";
        public string? Uom { get; set; }
        public decimal PackMultiplier { get; set; } = 1;
        public int CurrentStock { get; set; }

        public string Display => $"{Brand} {BaseName}".Trim();
    }

    // Mirrors the server's PosSaleLineExportRow (GET /api/sales/lines). SKU is nullable there:
    // a discount line has no inventory effect and carries no SKU.
    public class SaleLineExport
    {
        public int SaleNo { get; set; }
        public string ClientSaleId { get; set; } = string.Empty;
        public DateTime SoldAt { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public bool Voided { get; set; }
        public string? SKU { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public int ShortfallQty { get; set; }
    }

    public class RecipeLine
    {
        public string InputSku { get; set; } = string.Empty;
        public int Qty { get; set; }
    }

    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty; // "Baking" or "Decorating"
        public string OutputSku { get; set; } = string.Empty;
        public int OutputQty { get; set; }
        public bool IsActive { get; set; } = true;
        public List<RecipeLine> Lines { get; set; } = new();
    }
}
