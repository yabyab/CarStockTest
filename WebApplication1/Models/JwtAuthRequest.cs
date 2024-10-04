using System;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    [Serializable]
    public class JwtAuthRequest
    {
        public string? dealername { get; set; }
        public string? dealeremail { get; set; }
    }
}