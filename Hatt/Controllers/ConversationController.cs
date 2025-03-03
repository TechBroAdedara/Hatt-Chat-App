using Hatt.Dtos;
using Hatt.Models;
using Hatt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatt.Controllers
{
    [Route("conversations")]
    [ApiController]
    public class ConversationController(IConversationService conversationService) : ControllerBase
    {
        private readonly IConversationService _conversationService = conversationService;

        [HttpPost()]
        public async Task<IActionResult> CreateNewConversationAsync(ConversationDto conversation)
        {
            var newConversation = await _conversationService.CreateConversationAsync(conversation);
            return Ok(newConversation);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            try
            {
                var messages = await _conversationService.GetMessagesAsync(conversationId);
                return Ok(messages);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }
        }

        [HttpPost("{conversationId}/messages")]
        public async Task<IActionResult> AddMessageToConversationAsync(int conversationId, AddMessageDto message)
        {
            await _conversationService.AddMessageToConversationAsync(conversationId, message);
            return Ok(message);
        }
    }
}
