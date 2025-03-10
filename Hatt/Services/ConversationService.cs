using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;
using Hatt.Middleware;

namespace Hatt.Services;
public interface IConversationService
{
    Task<ConversationDto?> GetConversationByIdAsync(int Id);
    Task<ConversationDto> CreateConversationAsync(ConversationDto conversationDto);
    Task<List<Message>> GetMessagesAsync(int conversationId);
    Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageToAdd);

}
public class ConversationService(IConversationRepository conversationRepository) : IConversationService
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;

    //Task to create new conversation
    public async Task<ConversationDto> CreateConversationAsync(ConversationDto conversationDto)
    {
        var newConversation = await _conversationRepository.CreateConversationAsync(conversationDto);
        return newConversation;
    }

    //Task to Get all messages belonging to a conversation
    public async Task<List<Message>> GetMessagesAsync(int conversationId)
    {
        try
        {
            var messages = await _conversationRepository.GetMessagesAsync(conversationId);
            if (messages == null || !messages.Any())
            {
                throw new KeyNotFoundException("Conversation not found");
            }
            return [.. messages];
        }
        catch(KeyNotFoundException ex)
        {
            throw new HttpResponseException(404, ex.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //Task to add a new message to a conversation
    public async Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageDto)
    {
        try{
            if (await GetConversationByIdAsync(conversationId) == null)
            {
                throw new KeyNotFoundException("Conversation not found");
            }
            var message = await _conversationRepository.AddMessageToConversationAsync(conversationId, messageDto);
            return message;
        }
        catch (KeyNotFoundException ex)
        {
            throw new HttpResponseException(404, ex.Message);
        }
        catch (Exception)
        {
            throw;
        }
        
    }
    public async Task<ConversationDto?> GetConversationByIdAsync(int conversationId)
    {
        try
        {
            var existingConversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
            if (existingConversation == null)
            {
                throw new KeyNotFoundException("Conversation not found");
            }
            return existingConversation.AsDto();
        }
        catch (KeyNotFoundException ex)
        {
            throw new HttpResponseException(404, ex.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
