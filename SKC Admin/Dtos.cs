using System.ComponentModel;

namespace SKC_Admin
{
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

        // Sellable-at-POS is derived, not stored: the owner prices what branches may
        // sell (bread, candles) and leaves raw materials/intermediaries (flour, chiffon)
        // at 0. Category deliberately plays no part - see PUT /api/inventory/{sku}/price.
        public bool Sellable => Price > 0;

        // Combo-box display text only - hidden from the products grid via Browsable(false).
        [Browsable(false)]
        public string Display => $"{Brand} {BaseName}".Trim();
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
