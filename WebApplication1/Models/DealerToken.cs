namespace WebApplication1.Models
{
    public class DealerToken
    {
        public int id { get; set; }
        public int dealerid { get; set; }
        public required string token { get; set; }
        public required string expired_at { get; set; }

        public string Print(){
            return String.Format("Token ID: {0}, Dealer Id: {1}, token {2}, Expired_at: {3}", id ,dealerid, token, expired_at);
        }
    }
}