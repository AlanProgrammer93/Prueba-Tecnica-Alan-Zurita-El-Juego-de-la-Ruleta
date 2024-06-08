using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using mvc.Models;
using mvc.Services;
using mvc.Dto;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        [HttpPost("addBalance")]
        [Authorize]
        public async Task<IActionResult> UpdateBalance([FromBody] AddBalanceDto balance)
        {
            try
            {
                var username = User.Identity.Name;
                Console.WriteLine(balance.Balance);
                Console.WriteLine(username);
                var result = await _userService.UpdateBalance(username, balance.Balance);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        [HttpPost]
        [HttpPost("play")]
        [Authorize]
        public async Task<IActionResult> UpdateBalance([FromBody] PlayDto play)
        {
            try
            {
                var username = User.Identity.Name;
                Console.WriteLine(username);

                if (string.IsNullOrEmpty(play.EvenOdd) && !play.Number.HasValue) {
                    Console.WriteLine("apuesta solo color");
                } else if (!string.IsNullOrEmpty(play.EvenOdd)) {
                    Console.WriteLine("apuesta color y par");
                } else {
                    Console.WriteLine("apuesta color y numero");
                }

                await Task.Delay(2000);
                
                //var result = await _userService.UpdateBalance(username, balance.Balance);
                var result = 5;
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }
    }
}
