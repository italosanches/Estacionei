using Estacionei.DTOs.Account;
using Estacionei.DTOs.Authentication;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.UsersParameters;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseBase<TokenResponseDto>> Authentication(LoginRequestDto loginDto);
        Task<ResponseBase<bool>> Register(RegisterRequestDto registerDto);
        Task<ResponseBase<bool>> DeleteUser(string userEmail);
        Task<ResponseBase<TokenDto>> GenerateRefreshToken(TokenDto tokenDto);

        Task<ResponseBase<bool>> Revoke(string userEmail);

        Task<ResponseBase<bool>> CreateRole(string roleName);
        Task<ResponseBase<bool>> DeleteRole(string roleName);

        Task<ResponseBase<bool>> AddUserToRole(string email, string roleName);

        Task<ResponseBase<bool>> ChangePassword(ChangePasswordRequestDto changePasswordDto);

        Task<ResponseBase<PagedList<UserResponseDto>>> UsersPaginated(UsersParameters usersParameters);

    }

}
