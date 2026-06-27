using System;
using System.Collections.Generic;
using System.Text;

namespace SKC.DataEngine
{
    public class SaleRecord
    {
        public DateTime SaleDate { get; set; }
        public string Branch { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
