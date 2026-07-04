using System;
using System.Collections.Generic;
using System.Text;

namespace SKC_Bakery_Supplies
{
    public class DraftDeliveryItem
    {
        public string SKU { get; set; }
        public string Brand { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
    }

    public class DeliveryLog
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Date { get; set; }
        public string SKU { get; set; }
        public int Qty { get; set; }
        public string ToBranch { get; set; }
        public double TotalLineCost { get; set; }
        public string Requester { get; set; }
        public string Reason { get; set; }
    }

    public class DeliveryTicketSummary
    {
        public string TransactionId { get; set; }
        public string Date { get; set; }
        public string ToBranch { get; set; }
        public int TotalItems { get; set; }
    }

    public class InventoryLot
    {
        public int LotId { get; set; }
        public string SKU { get; set; }
        public string DateReceived { get; set; }
        public int OriginalQty { get; set; }
        public int RemainingQty { get; set; }
        public double UnitCost { get; set; }
    }
}
