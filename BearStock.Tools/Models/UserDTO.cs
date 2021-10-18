using System.Text.Json.Serialization;

namespace BearStock.Tools.Models
{
    public class UserDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("email")]

        public string Email { get; set; }
        [JsonPropertyName("username")]

        public string Username { get; set; }
    }
}