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
        public async Task<IActionResult> AddUser(AddUserDto userDto)
        {
            try{
                var result = await _userService.AddUserAsync(userDto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }            
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try{
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch(KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }
    }
}
