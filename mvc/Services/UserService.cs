using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Data;
using BCrypt.Net;
using mvc.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace mvc.Services
{
    public class UserService : IUserService
    {
        private readonly OfficeDb _dbContext;

        public UserService(OfficeDb dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserLoginReturnDto> CreateUserAsync(User user)
        {
            try
            {
                /* var userExist = await _dbContext.Users.FirstOrDefaultAsync(
                    user => user.Username == user.Username
                );

                if (userExist != null)
                {
                    throw new ArgumentNullException(nameof(userExist), "Ya existe el username ingresado.");
                } */

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHash;
                user.Balance = 0;
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                var token = GenerateJwtToken(user.Username);
                 
                UserLoginReturnDto result = new()
                {
                    user = user,
                    token = token
                };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserLoginReturnDto> UserLoginAsync(UserLoginDto data)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(
                    user => user.Username == data.Username
                );

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "No existe usuario activo");
                }

                bool isMatch = BCrypt.Net.BCrypt.Verify(data.Password, user.Password);
                if (!isMatch)
                {
                    throw new ArgumentException("No coinciden las credenciales");
                }

                var token = GenerateJwtToken(user.Username);
                 
                UserLoginReturnDto result = new()
                {
                    user = user,
                    token = token
                };

                return result;
            }
            catch
            {
                throw;
            }
        }
        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("clave_para_token");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> UpdateBalance(string username, float amount)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(
                    user => user.Username == username
                );
                user.Balance = user.Balance + amount;
                
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetUser(string username)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(
                    user => user.Username == username
                );
                return user;
            }
            catch
            {
                throw;
            }
        }
    }

    public interface IUserService
    {
        Task<UserLoginReturnDto> CreateUserAsync(User user);
        Task<UserLoginReturnDto> UserLoginAsync(UserLoginDto user);
        Task<User> UpdateBalance(string username, float amount);
        Task<User> GetUser(string username);
    }
}
