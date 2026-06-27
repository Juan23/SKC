using System;
using System.Collections.Generic;
using System.Text;

namespace SKC_Bakery_Supplies
{
    public class PurchaseLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SKU { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public string Supplier { get; set; }
        public string TransactionId { get; set; }
    }
}
