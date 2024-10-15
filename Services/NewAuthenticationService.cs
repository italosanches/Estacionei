using Estacionei.DTOs.Authentication;
using Estacionei.Models;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Estacionei.Services
{
    public class NewAuthenticationService : INewAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public NewAuthenticationService(ITokenService tokenService, 
                                    UserManager<ApplicationUser> userManager, 
                                    RoleManager<IdentityRole> roleManager, 
                                    IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ResponseBase<TokenResponseDto>> Authentication(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is not null && await _userManager.CheckPasswordAsync(user,loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//guid unico para o token atual
                };

                foreach (var role in userRoles) 
                { 
                    authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var token = _tokenService.GenerateAcessToken(authClaims,_configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // _ e uma variavel descarte, pois oque preciso e o out do tryparse
                _ = int.TryParse(_configuration["JwtSettings:RefreshTokenValidityInMinutes"],
                                                out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                var authenticationToken = new TokenResponseDto
                {  Token = token,
                   RefreshToken = refreshToken,
                   Expiration = _tokenService.GetExpirationFromToken(token)
                };
                return ResponseBase<TokenResponseDto>.SuccessResult(authenticationToken, "login realizado");
                
            }
            return ResponseBase<TokenResponseDto>.FailureResult("Email ou senha invalidos!",HttpStatusCode.Unauthorized);
        }


        public async Task<ResponseBase<TokenDto>> GenerateRefreshToken(TokenDto tokenDto)
        {
            if(tokenDto.Token is null)
            {
                return ResponseBase<TokenDto>.FailureResult($"{nameof(tokenDto.Token)} invalido",HttpStatusCode.BadRequest);
            }
            else if(tokenDto.RefreshToken is null)
            {
                return ResponseBase<TokenDto>.FailureResult($"{nameof(tokenDto.RefreshToken)} invalido", HttpStatusCode.BadRequest);
            }

            var claimsPrincipal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token, _configuration);
            if(claimsPrincipal == null)
            {
                return ResponseBase<TokenDto>.FailureResult($"Refresh/Token invalido", HttpStatusCode.BadRequest);
            }

       
            var userEmail = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken 
                             || user.RefreshTokenExpiryTime <= DateTime.Now) 
            { 
                return ResponseBase<TokenDto>.FailureResult($"Refresh/Token invalido", HttpStatusCode.BadRequest);

            }
            var newAcessToken = _tokenService.GenerateAcessToken(claimsPrincipal.Claims.ToList(), _configuration);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var token = new TokenDto 
            { Token = newAcessToken,RefreshToken =user.RefreshToken };
            return ResponseBase<TokenDto>.SuccessResult(token, "");
        }

        public async Task<ResponseBase<bool>> Register(RegisterDto registerDto)
        {
           var userExist = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExist != null) 
            {
                return ResponseBase<bool>.FailureResult("E-mail já existe", HttpStatusCode.InternalServerError);
            }

            ApplicationUser user = new ApplicationUser 
            {
              Email = registerDto.Email,
              SecurityStamp = Guid.NewGuid().ToString(),
              UserName = registerDto.Username
            };
            var result = await _userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded)
            {
                return ResponseBase<bool>.FailureResult("Criacao de usuario falhou", HttpStatusCode.InternalServerError);

            }
            var resultRole = await AddUserToRole(user.Email, "User");
            if(!resultRole.Data) 
            {
                return ResponseBase<bool>.FailureResult($"Erro ao vincular usuario a role User {resultRole.Message}", HttpStatusCode.InternalServerError);
            }
            return ResponseBase<bool>.SuccessResult(true,"Usuario criado e adicionado a role padrão.", HttpStatusCode.InternalServerError);

        }

        public async Task<ResponseBase<bool>> Revoke(string userEmail)
         {
           var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return ResponseBase<bool>.FailureResult("Email invalido.", HttpStatusCode.BadRequest);
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return ResponseBase<bool>.SuccessResult(true,"Acesso revogado",HttpStatusCode.NoContent);
        }

        public async Task<ResponseBase<bool>> CreateRole(string roleName)
        {
           var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if(!roleExists)
            {
                var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if(roleCreateResult.Succeeded)
                {
                    return ResponseBase<bool>.SuccessResult(true, "Role criada");
                }
                else
                {
                    return ResponseBase<bool>.FailureResult("Erro ao criar role.", HttpStatusCode.BadRequest);
                }
            }
            return ResponseBase<bool>.FailureResult($"Erro ao criar role, nome {roleName} ja existe no banco.", HttpStatusCode.BadRequest);


        }

        public async Task<ResponseBase<bool>> AddUserToRole(string email, string roleName)
        {
           var user       = await _userManager.FindByEmailAsync(email);
           var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists || user is null)
            { 
                return ResponseBase<bool>.FailureResult("Role ou e-mail nao existe.",HttpStatusCode.BadRequest);
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            { 
                return ResponseBase<bool>.SuccessResult(true, "Role vinculado ao usuario.");

            }
            else
            {
                return ResponseBase<bool>.FailureResult($"Erro ao vincular o usuario {email} a role {roleName}", HttpStatusCode.BadRequest);
            }
        }
    }
}
