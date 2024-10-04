namespace WebApplication1.Models{

[Serializable]
    public class JwtAuthResponse
    {
        public string? token { get; set; }
        public int user_id { get; set; }
        public string expired_at {get; set; }
    }
}