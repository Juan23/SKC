//BakeryProducts.cs

using System;
using System.Collections.Generic;
using System.Text;

namespace SKC_Bakery_Supplies
{
    public class BakeryProduct
    {
        public string SKU { get; set; }
        public string Brand { get; set; }
        public string BaseName { get; set; }
        // public string UOM { get; set; }
        // public decimal PackMultiplier { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int CurrentStock { get; set; }
        // used for the dropdown in Purchases.cs
        public string SearchDisplay => $"{Brand} {BaseName}".Trim();
    }
}