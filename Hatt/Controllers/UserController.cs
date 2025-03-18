using Hatt.Dtos;
using Hatt.Models;
using Hatt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("friends")]
        public async Task<IActionResult> GetUserFriends()
        {
            var userName = User.FindFirst("username")?.Value;
            var friends = await _userService.GetUserFriends(userName);
            return Ok(friends);
        }

        [HttpPost("friend-requests")]
        public async Task<IActionResult> SendFriendRequest(string recieverUsername)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sentRequest = await _userService.SendFriendRequest(senderId, recieverUsername);
            return Ok(sentRequest);
        }

        [HttpPut("friend-requests/{requestId}")]
        public async Task<IActionResult> UpdateFriendRequest([FromQuery]FriendshipStatusMotive motive, [FromRoute]int requestId)
        {
            var updateFriendship = await _userService.HandleFriendRequest(motive, requestId);
            return Ok(updateFriendship);
        }

        [HttpGet("friend-requests/recieved")]
        public async Task<IActionResult> GetPendingRecievedRequests()
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pendingRequests = await _userService.GetPendingRecievedRequests(senderId);
            return Ok(pendingRequests);
        }
        [HttpGet("friend-requests/sent")]
        public async Task<IActionResult> GetPendingSentRequests()
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pendingRequests = await _userService.GetPendingSentRequests(senderId);
            return Ok(pendingRequests);
        }
    }
}
