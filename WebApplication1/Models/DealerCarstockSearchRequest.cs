using System;

namespace WebApplication1.Models{
    public class DealerCarstockSearchRequest
    {
        public string? make { get; set; }
        public string? model { get; set; }
        public  int? year { get; set; }
    }
}
