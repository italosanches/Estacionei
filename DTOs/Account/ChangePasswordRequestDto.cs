using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Account
{
    public class ChangePasswordRequestDto
    {
        [JsonIgnore]
        public string? UserId { get; set; }

        public string Password { get; set; }
    }
}
