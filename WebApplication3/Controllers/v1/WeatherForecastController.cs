using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication3.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        //[HttpPost("login")]
        //[AllowAnonymous]
        //public IActionResult Login([FromBody] UserCredential user)
        //{
        //    if (user.Username == "admin" && user.Password == "123456") // Example validation
        //    {
        //        var claims = new[] {
        //            new Claim(ClaimTypes.Name, user.Username)
        //        };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for jwt token"));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            claims: claims,
        //            expires: DateTime.Now.AddMinutes(1),
        //            signingCredentials: creds
        //        );

        //        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //        return Ok(new { token = jwt });  // Return the JWT token
        //    }

        //    return Unauthorized();  // Return 401 if credentials are invalid
        //}

        [HttpGet]
        public IActionResult Get() => Ok("From v1");
    }

    //public record UserCredential(string Username, string Password);

}


