using System;
using System.Collections.Generic;

namespace SKC_Bakery_Supplies
{
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
}
