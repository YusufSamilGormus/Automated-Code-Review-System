using Microsoft.AspNetCore.Mvc;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase

    {
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("User Controller is working in .NET 9!");
        }
    }
}
