using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IConversationRepository{
    Task<ConversationDisplayDto> CreateConversationAsync(ConversationDto conversationDto);
    Task<Conversation?> GetConversationByIdAsync(Guid conversationId);
    Task<IEnumerable<Message>> GetMessagesAsync(Guid conversationId);
    Task<Message> AddMessageToConversationAsync(Guid conversationId, MessageDto message, string senderUserName);
    Task<ConversationUser> AddNewConversationUser(string userId, Guid conversationId);
    Task<ICollection<ConversationUser>> GetConversationUsers(Guid conversationId);

}
public class ConversationRepository(HattDbContext context) : IConversationRepository
{
    private readonly HattDbContext _context = context;

    //Create a new conversation once a friend request has been accepted. Invoked in the user service class
    public async Task<ConversationDisplayDto> CreateConversationAsync(ConversationDto conversationDto)
    {
       Conversation newConversation = new(){
        Name = conversationDto.Name,
        Type = conversationDto.Type,
        Participants = conversationDto.Participants

       };
        await _context.Conversations.AddAsync(newConversation);
        await _context.SaveChangesAsync();
        return newConversation.AsDisplayDto();
    }

    //Get conversation by its UUID
    public async Task<Conversation?> GetConversationByIdAsync(Guid conversationId)
    {
        var conversation = await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        return conversation;
    }

    //Get messages for a conversation
    public async Task<IEnumerable<Message>> GetMessagesAsync(Guid conversationId)
    {
        var messages = await _context.Messages.Where(m => m.ConversationId == conversationId).ToListAsync();
        return messages;
    }

    //Add a message to a conversation
    public async Task<Message> AddMessageToConversationAsync(Guid conversationId, MessageDto messageDto, string senderUserName)
    {
        Message newMessage = new(){
            SenderUsername = senderUserName,
            SentAt = DateTime.UtcNow,
            Content = messageDto.Content,
            ConversationId = conversationId
        };
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
        return newMessage;
    }

    public async Task<ConversationUser> AddNewConversationUser(string userId, Guid conversationId)
    {
        var newConversationUser = new ConversationUser
        {
            UserId = userId,
            ConversationId = conversationId
        };
        try
        {
            _context.ConversationsUsers.Add(newConversationUser);
            await _context.SaveChangesAsync();
            return newConversationUser;
        }
        catch (DbUpdateException)
        {
            throw;
        }
        
    }
    public async Task<ICollection<ConversationUser>> GetConversationUsers(Guid conversationId)
    {
        ICollection<ConversationUser> conversationUsers = await _context.ConversationsUsers
            .Where(cu => cu.ConversationId == conversationId)
            .Include(cu => cu.User)
            .Include(cu => cu.Conversation)
            .ToListAsync();
;
        return conversationUsers;
    }

}
