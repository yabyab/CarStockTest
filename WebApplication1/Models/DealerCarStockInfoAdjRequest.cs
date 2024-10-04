using System;

namespace WebApplication1.Models;

public class DealerCarStockInfoAdjRequest
{
    public int stockid { get; set; }
    public required string make { get; set; }
    public required string model { get; set; }
    public required int year { get; set; }

    public string Print(){
            return String.Format(@"Stock ID: {0}, Make: {1}, Model: {2}"
            , stockid, make, model);
    }
}
