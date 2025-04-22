using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("3.0")]
    [Authorize]
    public class EmailController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("From v3");
    }

}
