namespace WebApplication1.Models{

    public class DealerCarStockPriceAdjRequest
    {
        public int stockid { get; set; }
        public required decimal price { get; set; }
    }
}