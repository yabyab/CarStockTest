namespace WebApplication1.Models
{
    public class Dealer
    {
        // public int dealerid { get; set; }
        public required string dealername { get; set; }
        public required string dealeremail { get; set; }

        public string Print(){
            return String.Format("Dealer Name: {1}, Email {2}",  dealername, dealeremail);
        }
    }
}