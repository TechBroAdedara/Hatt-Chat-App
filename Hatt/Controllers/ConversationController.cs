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
    [Route("conversations")]
    [ApiController]
    public class ConversationController(IConversationService conversationService) : ControllerBase
    {
        private readonly IConversationService _conversationService = conversationService;

        [HttpPost()]
        public async Task<IActionResult> CreateNewConversationAsync(ConversationDto conversation)
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var newConversation = await _conversationService.CreateConversationAsync(Id,conversation);
            return Ok(newConversation);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {

            var messages = await _conversationService.GetMessagesAsync(conversationId);
            return Ok(messages);


        }

        [HttpPost("{conversationId}/messages")]
        public async Task<IActionResult> AddMessageToConversationAsync(int conversationId, AddMessageDto message)
        {
            var userName = User.FindFirst("username")?.Value;

            await _conversationService.AddMessageToConversationAsync(conversationId, message, userName);
            return Ok(message);
        }
    }
}
