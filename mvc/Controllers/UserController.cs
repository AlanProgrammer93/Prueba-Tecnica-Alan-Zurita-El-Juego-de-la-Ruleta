using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using mvc.Models;
using mvc.Services;
using mvc.Dto;

namespace mvc.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var result = await _userService.CreateUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto user)
        {
            try
            {
                var result = await _userService.UserLoginAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
