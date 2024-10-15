using Estacionei.DTOs.Authentication;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly INewAuthenticationService _authenticationService;

        public AuthenticationController(INewAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
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
        [HttpPost("Revoke")]
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
        [HttpPost("Role")]
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
        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(string userEmail, string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.AddUserToRole(userEmail, roleName);

            return StatusCode((int)result.StatusCode,result.Message);
        }
    }
}
