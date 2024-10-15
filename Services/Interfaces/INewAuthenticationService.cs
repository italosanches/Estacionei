using Estacionei.DTOs.Authentication;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface INewAuthenticationService
    {
        Task<ResponseBase<TokenResponseDto>> Authentication(LoginDto loginDto);
        Task<ResponseBase<bool>> Register(RegisterDto registerDto);

        Task<ResponseBase<TokenDto>> GenerateRefreshToken(TokenDto tokenDto);

        Task<ResponseBase<bool>> Revoke(string userEmail);

        Task<ResponseBase<bool>> CreateRole(string roleName);

        Task<ResponseBase<bool>> AddUserToRole(string email, string roleName);
    }

}
