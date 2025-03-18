using System;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;

namespace Hatt.Services;
public interface IConversationService
{   
    Task<ConversationDisplayDto> CreateConversationAsync(string userId, ConversationDto conversationDto);
    Task<ConversationDto?> GetConversationByIdAsync(int Id);
    Task<List<Message>> GetMessagesAsync(int conversationId);
    Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageToAdd, string senderUserName);
    
    Task<List<User>> GetUsersInConversation(int conversationId);
}

//IMPLEMENTATION
public class ConversationService(IConversationRepository conversationRepository) : IConversationService
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;

    //Task to create new conversation
    public async Task<ConversationDisplayDto> CreateConversationAsync(string userId, ConversationDto conversationDto)
    {
        var newConversation = await _conversationRepository.CreateConversationAsync(conversationDto);
        await AddNewConversationUser(userId, newConversation.Id);
        return newConversation;
    }

    //Task to Get all messages belonging to a conversation
    public async Task<List<Message>> GetMessagesAsync(int conversationId)
    {
        
        var messages = await _conversationRepository.GetMessagesAsync(conversationId);
        if (messages == null || !messages.Any())
        {
            throw new KeyNotFoundException("Conversation not found");
        }
        return [.. messages];

    }

    //Task to add a new message to a conversation
    public async Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageDto, string senderUserName)
    {
      
        if (await GetConversationByIdAsync(conversationId) == null)
        {
            throw new KeyNotFoundException("Conversation not found");
        }
        var message = await _conversationRepository.AddMessageToConversationAsync(conversationId, messageDto, senderUserName);
        return message;
        
        
    }
    public async Task<ConversationDto?> GetConversationByIdAsync(int conversationId)
    {
        
        var existingConversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
        if (existingConversation == null)
        {
            throw new KeyNotFoundException("Conversation not found");
        }
        return existingConversation.AsDto();
        
    }

    private async Task AddNewConversationUser(string userId, int conversationId)
    {
        try { await _conversationRepository.AddNewConversationUser(userId, conversationId); }
        catch {
            throw;
        };
    }

    //Not used by any other class for now
    public async Task<List<User>> GetUsersInConversation(int conversationId)
    {
        var users = await _conversationRepository.GetUsersInConversation(conversationId);
        return users;
    }
}
