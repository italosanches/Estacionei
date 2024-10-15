using Estacionei.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Estacionei.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration config)
        {
            var key = config["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("Chave secreta inválida.");
            var privateKey = Encoding.UTF8.GetBytes(key);//transforma em array de bytes
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);//criando a assinatura

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["JwtSettings:TokenValidityInMinutes"])),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128]; //criando bytes aleatorios
            using var randomNumberGenerator = RandomNumberGenerator.Create(); //criando uma instancia  do gerador de números aleatórios seguro
            randomNumberGenerator.GetBytes(secureRandomBytes);// popula o array de bytes com os numeros gerados
            var refreshToken = Convert.ToBase64String(secureRandomBytes); // transforma  o array  em uma string em base64
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration config)
        {
            var secretKey = config["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("Chave secreta invalida.");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                                      Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                       out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                             !jwtSecurityToken.Header.Alg.Equals(
                             SecurityAlgorithms.HmacSha256,
                             StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal; //principal equivale as claims
        }
        public DateTime? GetExpirationFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                throw new ArgumentException("Token inválido.");

            var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim != null)
            {
                var exp = long.Parse(expClaim.Value);
                return DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            }

            return null;
        }

    }
}
