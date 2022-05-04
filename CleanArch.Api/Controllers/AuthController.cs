using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return Unauthorized();
        }
    }
}
