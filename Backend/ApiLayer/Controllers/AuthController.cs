using Azure.Core;
using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(JwtService jwtService, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO userDTO)
        {
            if (!clsUser.Login(userDTO))
            {
                return Unauthorized("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userDTO.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            //var accessToken = _jwtService.GenerateAccessToken(claims, (int)_configuration["Jwt:LifeTime"]);
            var accessToken = _jwtService.GenerateAccessToken(claims, 15);
            var refreshToken = _jwtService.GenerateRefreshToken();
            clsRefreshToken.SaveRefreshToken(new RefreshTokenDTO(0, refreshToken, userDTO.Id));

            Response.Cookies.Append("refreshToken", refreshToken, new
            CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new {
                AccessToken = accessToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15)
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized("Invalid refresh token");
            }
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Invalid refresh token");
            }
            var storedToken = clsRefreshToken.GetRefreshToken(refreshToken);
            if (storedToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            var userDTO = clsUser.GetUserById(storedToken.UserId);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userDTO.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var newAccessToken = _jwtService.GenerateAccessToken(claims, 15);
            return Ok(new
            {
                AccessToken = newAccessToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15)
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

    }
}
