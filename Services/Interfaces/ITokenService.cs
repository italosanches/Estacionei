using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Estacionei.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration config);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token,IConfiguration config);

        DateTime? GetExpirationFromToken(string token);
    }
}
