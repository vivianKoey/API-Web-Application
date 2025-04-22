using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ApiVersioning.Models;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [ApiVersion("4.0")] // Support multiple API versions
    public class AuthController : ControllerBase
    {
        private static Dictionary<string, string> refreshTokenStore = new Dictionary<string, string>();
        
        [HttpPost("login")]
        [AllowAnonymous] // Login is publicly accessible
        public IActionResult Login([FromBody] UserCredential user)
        {
            if (user.Username == "admin" && user.Password == "123456") // Example validation
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for jwt token"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var accessToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30), // Token expiration
                    signingCredentials: creds
                );

                var jwtAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

                var refreshToken = GenerateRefreshToken();

                refreshTokenStore[user.Username] = refreshToken;
                return Ok(new 
                { token = jwtAccessToken,
                   refreshToken = refreshToken});
            }

            return Unauthorized(); // Invalid credentials
        }

        [HttpGet("secure-data")]
        [Authorize] // Requires valid JWT token
        public IActionResult SecureData()
        {
            return Ok("This is a secure endpoint, and you are authorized!");
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }

            // Validate the refresh token: Check if it exists in the store
            var user = refreshTokenStore.FirstOrDefault(x => x.Value == request.RefreshToken).Key;
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { message = "Invalid refresh token." });
            }

            // Generate new claims for the user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, "Administrator") // Example role-based claim
            };

            // Generate a new access token (expires in 30 minutes)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for jwt token"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var newAccessToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );
            var jwtAccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

            // Generate a new refresh token
            var newRefreshToken = GenerateRefreshToken();

            // Replace the old refresh token with the new one
            refreshTokenStore[user] = newRefreshToken;

            // Return the new tokens
            return Ok(new
            {
                AccessToken = jwtAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        private string GenerateRefreshToken()
        {
            // Example: Generate a cryptographically secure random refresh token
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var byteArray = new byte[32];
                rng.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }
        }
    }

    public record UserCredential(string Username, string Password);
}