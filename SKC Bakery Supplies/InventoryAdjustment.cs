using System;

namespace SKC_Bakery_Supplies
{
    public class InventoryAdjustment
    {
        public DateTime Date { get; set; }
        public string SKU { get; set; }
        public string Brand { get; set; }
        public string BaseName { get; set; }
        public int QtyDelta { get; set; }
        public decimal UnitCost { get; set; }
        public string Reason { get; set; }
    }
}
