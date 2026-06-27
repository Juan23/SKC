using System;
using System.Collections.Generic;
using System.Text;

namespace SKC_Bakery_Supplies
{
    public class PurchaseTicketSummary
    {
        public string TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Supplier { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
