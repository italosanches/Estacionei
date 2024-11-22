using Microsoft.AspNetCore.Authentication;

namespace Estacionei.DTOs.Authentication
{
    public class TokenResponseDto : TokenDto
    {
        public DateTime? Expiration { get; set; }
    }
}
