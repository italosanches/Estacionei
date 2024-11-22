using AutoMapper;
using Estacionei.DTOs.Account;
using Estacionei.DTOs.Authentication;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.UsersParameters;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Estacionei.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(ITokenService tokenService,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration,
                              IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseBase<TokenResponseDto>> Authentication(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is not null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//guid unico para o token atual
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var token = _tokenService.GenerateAcessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // _ e uma variavel descarte, pois oque preciso e o out do tryparse
                _ = int.TryParse(_configuration["JwtSettings:RefreshTokenValidityInMinutes"],
                                                out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                var authenticationToken = new TokenResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    Expiration = _tokenService.GetExpirationFromToken(token)
                };
                return ResponseBase<TokenResponseDto>.SuccessResult(authenticationToken, "login realizado");

            }
            return ResponseBase<TokenResponseDto>.FailureResult("Email ou senha invalidos!", HttpStatusCode.Unauthorized);
        }


        public async Task<ResponseBase<TokenDto>> GenerateRefreshToken(TokenDto tokenDto)
        {
            if (tokenDto.Token is null)
            {
                return ResponseBase<TokenDto>.FailureResult($"{nameof(tokenDto.Token)} invalido", HttpStatusCode.BadRequest);
            }
            else if (tokenDto.RefreshToken is null)
            {
                return ResponseBase<TokenDto>.FailureResult($"{nameof(tokenDto.RefreshToken)} invalido", HttpStatusCode.BadRequest);
            }

            var claimsPrincipal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token, _configuration);
            if (claimsPrincipal == null)
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
            { Token = newAcessToken, RefreshToken = user.RefreshToken };
            return ResponseBase<TokenDto>.SuccessResult(token, "");
        }

        public async Task<ResponseBase<bool>> Register(RegisterRequestDto registerDto)
        {
            var userEmailExist = await _userManager.FindByEmailAsync(registerDto.Email);
            var userNameExist = await _userManager.FindByNameAsync(registerDto.Username);
            if (userEmailExist != null || userNameExist != null)
            {
                return ResponseBase<bool>.FailureResult("E-mail ou usuario já existe", HttpStatusCode.InternalServerError);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username
            };

            if (!await ValidatePassword(user, registerDto.Password))
            {
                return ResponseBase<bool>.FailureResult("Senha não possui os parametros necessários.Precisa de um digito, comprimento mínimo 6, letras maiúsculas e minúsculas.", HttpStatusCode.BadRequest);
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return ResponseBase<bool>.FailureResult("Criacao de usuario falhou", HttpStatusCode.InternalServerError);

            }
            var resultRole = await AddUserToRole(user.Email, "User");
            if (!resultRole.Data)
            {
                return ResponseBase<bool>.FailureResult($"Erro ao vincular usuario a role User {resultRole.Message}", HttpStatusCode.InternalServerError);
            }
            return ResponseBase<bool>.SuccessResult(true, "Usuario criado e adicionado a role padrão.");



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
            return ResponseBase<bool>.SuccessResult(true, "Acesso revogado", HttpStatusCode.NoContent);
        }

        public async Task<ResponseBase<bool>> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleCreateResult.Succeeded)
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
            var user = await _userManager.FindByEmailAsync(email);
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists || user is null)
            {
                return ResponseBase<bool>.FailureResult("Role ou e-mail nao existe.", HttpStatusCode.BadRequest);
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




        public async Task<ResponseBase<bool>> ChangePassword(ChangePasswordRequestDto changePasswordDto)
        {
            var userSearched = await _userManager.FindByIdAsync(changePasswordDto.UserId);
            if (userSearched is null)
            {
                return ResponseBase<bool>.FailureResult("Usuário não encontrado.", HttpStatusCode.BadRequest);
            }
            if (!await ValidatePassword(userSearched, changePasswordDto.Password)) 
            { 
                return ResponseBase<bool>.FailureResult("Senha não possui os parametros necessários. Precisa de um digito, comprimento mínimo 6, letras maiúsculas e minúsculas.", HttpStatusCode.BadRequest);
            }
            var hashPassword = await _userManager.GeneratePasswordResetTokenAsync(userSearched);

            var result = await _userManager.ResetPasswordAsync(userSearched, hashPassword, changePasswordDto.Password);
            if (!result.Succeeded)
            {
                return ResponseBase<bool>.FailureResult("Erro ao alterar a senha." + result.Errors.First(), HttpStatusCode.BadRequest);
            }
            return ResponseBase<bool>.SuccessResult(true, "Senha alterada.");
        }

        public async Task<ResponseBase<bool>> DeleteUser(string userEmail)
        {
            var userSearched = await _userManager.FindByEmailAsync(userEmail);
            if(userSearched is null)
            {
                return ResponseBase<bool>.FailureResult("Usuário não encontrado.",HttpStatusCode.NotFound);
            }
            var result = await _userManager.DeleteAsync(userSearched);
            if(!result.Succeeded)
            {
                return ResponseBase<bool>.FailureResult($"Erro ao remover usuário.{string.Join(",",result.Errors)}", HttpStatusCode.NotFound);
            }
            return ResponseBase<bool>.SuccessResult(true,"Usuário removido.");

        }

        public async Task<ResponseBase<bool>> DeleteRole(string roleName)
        {
           var roleSearched = await _roleManager.FindByNameAsync(roleName);
            if (roleSearched is null)
            {
                return ResponseBase<bool>.FailureResult("Role não encontrada.", HttpStatusCode.NotFound);
            }
           var result = await _roleManager.DeleteAsync(roleSearched);
            if (!result.Succeeded)
            {
                return ResponseBase<bool>.FailureResult($"Erro ao remover role.{string.Join(",", result.Errors)}", HttpStatusCode.NotFound);
            }
            return ResponseBase<bool>.SuccessResult(true, "Role removida.");

        }


        private async Task<bool> ValidatePassword(ApplicationUser user, string password)
        {
            var validatePassword = _userManager.PasswordValidators;

            foreach (var validator in validatePassword)
            {
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                {
                    return false;
                }

            }
            return true;
        }

        public async Task<ResponseBase<PagedList<UserResponseDto>>> UsersPaginated(UsersParameters usersParameters)
        {
            var usersQueryable = _userManager.Users;
            var usersPaginated = await PagedListService<UserResponseDto,ApplicationUser>.CreatePagedList(usersQueryable, usersParameters,_mapper);
            return ResponseBase<PagedList<UserResponseDto>>.SuccessResult(usersPaginated,"");
        }
    }
}
