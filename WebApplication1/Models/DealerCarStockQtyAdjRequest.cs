using System;

namespace WebApplication1.Models{

    public class DealerCarStockQtyAdjRequest
    {
        public int stockid { get; set; }
        public required int quantity { get; set; }
    }
}