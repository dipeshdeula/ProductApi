using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Repositories;
using ProductApi.Services;
using ProductApi.Utilities;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly JwtTokenHelper _jwtTokenHelper;
        

        public AuthController(IUserService userService,JwtTokenHelper jwtTokenHelper)
        {
            _userService = userService;
            _jwtTokenHelper = jwtTokenHelper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtTokenHelper.GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize(Roles="Admin")]
        [HttpGet("Secure-data")]
        public async Task<IActionResult> GetSecureDataAsync()
        {
            var getToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //return Ok("This is secure data");
            await Task.CompletedTask;
            return Ok(new { getToken });
            
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("admin-data")]
        public async Task<IActionResult> GetAdminDataAsync()
        {
            var result = await Task.FromResult("This is admin data");
            return Ok(result);
        }
    }
}
