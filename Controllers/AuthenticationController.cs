using Estacionei.DTOs.Account;
using Estacionei.DTOs.Authentication;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.UsersParameters;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Estacionei.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _authenticationService;

        public AuthenticationController(IAccountService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.Authentication(loginDto);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return Unauthorized(result.Message);
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.Register(registerDto);

            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, "Usuario criado.");
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [Authorize(Policy ="AdminOnly")]
        [HttpDelete("user/{userEmail}")]
        public async Task<IActionResult> DeleteUser (string userEmail)
        {
            if(userEmail is null)
            {
                return BadRequest("Email está vazio.");
            }
            var result = await _authenticationService.DeleteUser(userEmail);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDto token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.GenerateRefreshToken(token);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(string userEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.Revoke(userEmail);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
            return StatusCode((int)result.StatusCode, result.Message);


        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.CreateRole(roleName);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(string userEmail, string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.AddUserToRole(userEmail, roleName);

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [Authorize(Policy ="AdminOnly")]
        [HttpDelete("role/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if(roleName == null)
            {
                return BadRequest("Nome da role esta vazio.");
            }
            var result = await _authenticationService.DeleteRole(roleName);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            changePasswordDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (changePasswordDto.UserId is null)
            {
                return StatusCode(500,("Erro ao processar a solicitação"));
            }
            var result =await _authenticationService.ChangePassword(changePasswordDto);

            return StatusCode((int)result.StatusCode,(result.Message));
        }

        [Authorize(Policy ="AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery]UsersParameters usersParameters)
        {
            var result = await _authenticationService.UsersPaginated(usersParameters);
            var paginationMetadata = PaginationMetadata<UserResponseDto>.CreatePaginationMetadata(result.Data);
            Response.Headers.Append("X-Pagination",JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result.Data);
        }


    }
}
