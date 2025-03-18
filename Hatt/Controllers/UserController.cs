using Hatt.Dtos;
using Hatt.Models;
using Hatt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatt.Controllers
{
    [Authorize]
    [Route("users")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;


        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
           
        }

        [HttpGet("UserName/{UserName}")]
        public async Task<IActionResult> GetUserByUserNameAsync(string UserName)
        {
            var user = await _userService.GetUserByUserNameAsync(UserName);
            return Ok(user);
        }
    }
}
