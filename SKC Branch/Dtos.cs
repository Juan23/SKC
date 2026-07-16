using System;
using System.Collections.Generic;

namespace SKC_Branch
{
    public class BranchProduct
    {
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BaseName { get; set; } = string.Empty;
    }

    public class DeliveryTicketSummary
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string ToBranch { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public string Requester { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public double TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DeliveryLog
    {
        public int Id { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Qty { get; set; }
        public string ToBranch { get; set; } = string.Empty;
        public double TotalLineCost { get; set; }
        public string Requester { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class BranchStockItem
    {
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BaseName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    // ---- POS (offline-first sales) ----

    public class PosSaleLineDto
    {
        public string? SKU { get; set; } // null = discount line (no inventory effect)
        public string Description { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class PosSaleDto
    {
        public string ClientSaleId { get; set; } = string.Empty; // GUID minted at the counter
        public string Branch { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public DateTime SoldAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PosSaleLineDto> Lines { get; set; } = new();
    }

    public class SaleSyncResult
    {
        public string ClientSaleId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Synced | AlreadySynced | SyncedWithShortfall | Rejected
        public string Detail { get; set; } = string.Empty;
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
        public bool IsActive { get; set; }
        public List<RecipeLine> Lines { get; set; } = new();
    }

    public class ProductionResult
    {
        public string OutputSku { get; set; } = string.Empty;
        public int OutputQty { get; set; }
        public decimal TotalInputCost { get; set; }
    }

    public class ProductionBatch
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public decimal BatchMultiplier { get; set; }
        public string OutputSku { get; set; } = string.Empty;
        public int OutputQty { get; set; }
        public decimal TotalInputCost { get; set; }
    }
}
