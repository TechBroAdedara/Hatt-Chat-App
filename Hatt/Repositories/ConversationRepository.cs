using System;
using Hatt.Data;
using Hatt.Dtos;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Repositories;
public interface IConversationRepository{
    Task<ConversationDto> CreateConversationAsync(ConversationDto conversationDto);
    Task<Conversation?> GetConversationByIdAsync(int conversationId);
    Task<IEnumerable<Message>> GetMessagesAsync(int conversationId);
    Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto message);

}
public class ConversationRepository(HattDbContext context) : IConversationRepository
{
    private readonly HattDbContext _context = context;

    public async Task<ConversationDto> CreateConversationAsync(ConversationDto conversationDto)
    {
       Conversation newConversation = new(){
        Name = conversationDto.Name,
        Type = conversationDto.Type,
        Participants = conversationDto.Participants

       };
        await _context.Conversations.AddAsync(newConversation);
        await _context.SaveChangesAsync();
        return conversationDto;
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
    public async Task<AddMessageDto> AddMessageToConversationAsync(int conversationId, AddMessageDto messageDto)
    {
        Message newMessage = new(){
            Sender_id = messageDto.Sender_id,
            SentAt = messageDto.SentAt,
            Content = messageDto.Content,
            ConversationId = conversationId
        };
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
        return messageDto;
    }
}
