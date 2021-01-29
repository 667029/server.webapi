using Expenses.Core;
using Expenses.DB;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expenses.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            var result = await _userService.SignUp(user);
            return Created("", result);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            var result = await _userService.SignIn(user);
            return Ok(result);
        }
    }
}
