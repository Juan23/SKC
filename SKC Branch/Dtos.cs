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
}
