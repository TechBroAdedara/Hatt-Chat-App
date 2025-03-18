using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IConversationRepository{
    Task<ConversationDisplayDto> CreateConversationAsync(ConversationDto conversationDto);
    Task<Conversation?> GetConversationByIdAsync(int conversationId);
    Task<IEnumerable<Message>> GetMessagesAsync(int conversationId);
    Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto message, string senderUserName);
    Task AddNewConversationUser(string userId, int conversationId);
    Task<List<User>> GetUsersInConversation(int conversationId);

}
public class ConversationRepository(HattDbContext context) : IConversationRepository
{
    private readonly HattDbContext _context = context;

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
    public async Task<Conversation?> GetConversationByIdAsync(int conversationId)
    {
        var conversation = await _context.Conversations.FindAsync(conversationId);
        return conversation;
    }
    public async Task<IEnumerable<Message>> GetMessagesAsync(int conversationId)
    {
        var messages = await _context.Messages.Where(m => m.ConversationId == conversationId).ToListAsync();
        return messages;
    }
    public async Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageDto, string senderUserName)
    {
        Message newMessage = new(){
            SenderUsername = senderUserName,
            SentAt = DateTime.UtcNow,
            Content = messageDto.Content,
            ConversationId = conversationId
        };
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
        return messageDto;
    }

    public async Task AddNewConversationUser(string userId, int conversationId)
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
        }
        catch (DbUpdateException)
        {
            throw;
        }
        
    }
    public async Task<List<User>> GetUsersInConversation(int conversationId)
    {
        var users = await _context.ConversationsUsers.Where(c => c.ConversationId == conversationId)
            .Select(c => c.User)
            .ToListAsync();
        return users;
    }

}
