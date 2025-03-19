using System;
using System.Globalization;
using Hatt.Dtos;
using Hatt.Models;
using Hatt.Repositories;

namespace Hatt.Services;
public interface IConversationService
{   
    Task<ConversationDisplayDto> CreateConversationAsync(string userId1, string userId2, int participants, ConversationType type);
    Task<Conversation?> GetConversationByIdAsync(Guid conversationId);
    Task<List<DisplayMessageDto>> GetMessagesAsync(Guid conversationId);
    Task<MessageDto> AddMessageToConversationAsync(Guid conversationId, MessageDto messageToAdd, string senderUserName);
    Task<ConversationUserDto> AddNewConversationUser(string userId, Guid conversationId);
    Task<ICollection<ConversationUserDto>> GetConversationUsers(Guid conversationId);
}

//IMPLEMENTATION
public class ConversationService(IConversationRepository conversationRepository) : IConversationService
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;

    //Task to create new conversation
    public async Task<ConversationDisplayDto> CreateConversationAsync(string userId, string userId2, int participants, ConversationType type)
    {
        var newConversation = new ConversationDto
        (
            Name: "New Conversation", //Hard Coded. Have to fix
            Type: type,
            Participants: participants
        );
        var addedConversation = await _conversationRepository.CreateConversationAsync(newConversation);

        return addedConversation;
    }


    //Task to Get all messages belonging to a conversation
    public async Task<List<DisplayMessageDto>> GetMessagesAsync(Guid conversationId)
    {
        var conversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
        if (conversation == null)
        {
            throw new KeyNotFoundException("Conversation not found");
        }

        return [.. conversation.Messages.Select(m => m.AsDisplayDto())];

    }

    //Task to add a new message to a conversation, after it has been sent to reciepient
    public async Task<MessageDto> AddMessageToConversationAsync(Guid conversationId, MessageDto messageDto, string senderUserName)
    {
      
        if (await GetConversationByIdAsync(conversationId) == null)
        {
            throw new KeyNotFoundException("Conversation not found");
        }
        var message = await _conversationRepository.AddMessageToConversationAsync(conversationId, messageDto, senderUserName);
        return message.AsMessageDto();
        
        
    }

    //Getting a conversation by Id
    public async Task<Conversation?> GetConversationByIdAsync(Guid conversationId)
    {
        
        var existingConversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
        if (existingConversation == null)
        {
            throw new KeyNotFoundException("Conversation not found");
        }
        return existingConversation;
        
    }

    //Add new users to a conversation
    public async Task<ConversationUserDto> AddNewConversationUser(string userId, Guid conversationId)
    {

        Conversation? conversation = await GetConversationByIdAsync(conversationId);
        //Gets the users already in the conversation
        ICollection<ConversationUser> conversationUsers = await _conversationRepository.GetConversationUsers(conversationId);

        //If the users in a public conversation are at their maximum, throw an exception when attempting to add more users
        if((conversation?.Type == ConversationType.Private) && conversationUsers.Count == 2)
        {
            throw new InvalidOperationException("This is a private chat. Can't add more users");
        }

        ConversationUser addedConversationUser = await _conversationRepository.AddNewConversationUser(userId, conversationId);
        return addedConversationUser.ToConversationUserDto();
        

    }

    //Getting users in a conversation
    public async Task<ICollection<ConversationUserDto>> GetConversationUsers(Guid conversationId)
    {
        ICollection<ConversationUserDto> conversationUsers = (await _conversationRepository.GetConversationUsers(conversationId))
            .Select(cu => cu.ToConversationUserDto())
            .ToList();
        return conversationUsers;
    }
}
