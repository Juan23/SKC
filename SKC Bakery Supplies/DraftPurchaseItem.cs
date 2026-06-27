using System;
using System.Collections.Generic;
using System.Text;

namespace SKC_BakerySupplies
{
    public class DraftPurchaseItem
    {
        public string SKU { get; set; }
        public string Brand { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total => Qty * UnitCost;
    }
}
