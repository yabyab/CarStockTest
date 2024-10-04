namespace WebApplication1.Models;

public class DealerCarStock
{
    public int stockid { get; set; }
    public required int dealerid { get; set; }
    public required string make { get; set; }
    public required string model { get; set; }
    public required int year { get; set; }
    public required decimal price { get; set; }
    public required int quantity { get; set; }

    public string Print(){
            return String.Format(@"Stock ID: {0}, Dealer ID: {1}, Make: {2}, Model: {3}, Price: {4}, year: {5}, Qty: {6}"
            , stockid, dealerid, make, model, year, price, quantity);
    }
}
