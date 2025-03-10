using Hatt.Dtos;
using Hatt.Models;
using Hatt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hatt.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            return Ok(result);

        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
           
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
    }
}
