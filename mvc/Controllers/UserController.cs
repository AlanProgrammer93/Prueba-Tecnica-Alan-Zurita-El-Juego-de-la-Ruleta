using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using mvc.Models;
using mvc.Services;
using mvc.Dto;
using Microsoft.AspNetCore.Authorization;
using System;

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
        public async Task<IActionResult> Play([FromBody] PlayDto play)
        {
            try
            {
                var username = User.Identity.Name;

                string[] arrayEvenOdd = new string[] { "Par", "Impar" };
                string[] arrayColor = new string[] { "Negro", "Rojo" };

                int winningNumber = GetRandomNumber(0, 37);
                int indexColor = GetRandomNumber(0, 2);
                int indexEvenOdd = GetRandomNumber(0, 2);

                string resultBet = "";
                float won = 0;

                var updatedBalance = await _userService.GetUser(username);

                if (string.IsNullOrEmpty(play.EvenOdd) && !play.Number.HasValue) {
                    if (arrayColor[indexColor] == play.Color) {
                        won = play.Amount / 2;
                        //updatedBalance = await _userService.UpdateBalance(username, won);
                        resultBet = "Ganaste ";
                    } else {
                        updatedBalance = await _userService.UpdateBalance(username, -play.Amount);
                        resultBet = "Perdiste ";
                    }
                } else if (!string.IsNullOrEmpty(play.EvenOdd)) {
                    if (arrayColor[indexColor] == play.Color && arrayEvenOdd[indexEvenOdd] == play.EvenOdd) {
                        won = play.Amount;
                        //updatedBalance = await _userService.UpdateBalance(username, play.Amount);
                        resultBet = "Ganaste ";
                    } else {
                        updatedBalance = await _userService.UpdateBalance(username, -play.Amount);
                        resultBet = "Perdiste ";
                    }
                } else {
                    if (arrayColor[indexColor] == play.Color && winningNumber == play.Number) {
                        won = play.Amount * 3;
                        //updatedBalance = await _userService.UpdateBalance(username, won);
                        resultBet = "Ganaste ";
                    } else {
                        updatedBalance = await _userService.UpdateBalance(username, -play.Amount);
                        resultBet = "Perdiste ";
                    }
                }

                await Task.Delay(2000);
                
                ResultBetDto result = new()
                {
                    Color = arrayColor[indexColor],
                    EvenOdd = arrayEvenOdd[indexEvenOdd],
                    Number = winningNumber,
                    Result = resultBet,
                    Won = won,
                    User = updatedBalance
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        private int GetRandomNumber(int minValue, int maxValue)
        {
            Random random = new Random();
            return random.Next(minValue, maxValue);
        }
    }
}
