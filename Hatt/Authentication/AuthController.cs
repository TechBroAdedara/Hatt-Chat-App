using Hatt.Dtos;
using Hatt.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hatt.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _auth = authService;

        [HttpPost("register")]
        public async Task<ActionResult<UserDisplayDto>> Register([FromBody] RegisterModel model)
        {
            var user = await _auth.CreateUserAsync(model);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            var token = await _auth.Login(model);
            return Ok(token);
        }
    }
}
