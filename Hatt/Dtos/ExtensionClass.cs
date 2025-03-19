using System;
using System.Runtime.CompilerServices;
using Hatt.Models;

namespace Hatt.Dtos;

public static class ExtensionClass
{
    public static ConversationDto AsDto(this Conversation conversation)
    {
        return new ConversationDto(
            conversation.Name,
            conversation.Type,
            conversation.Participants
        );
    }
    public static ConversationUserDto ToConversationUserDto(this ConversationUser conversationUser)
    {
        return new ConversationUserDto(
            conversationUser.UserId,
            conversationUser.ConversationId,
            conversationUser.Role
            );
    }
    public static UserDisplayDto ToUserDisplayDto(this User user)
    {
        return new UserDisplayDto(
            user.Firstname,
            user.Lastname,
            user.Email,
            user.UserName
        );
    }
    
    public static ConversationDisplayDto AsDisplayDto(this Conversation conversation)
    {
        return new ConversationDisplayDto
        (
            conversation.Id,
            conversation.Name,
            conversation.Type,
            conversation.Participants
        );
    }
    public static FriendRequestDisplayDto AsDisplayDto(this FriendRequest friendRequest)
    {
        return new FriendRequestDisplayDto
            (
            friendRequest.Id,
            friendRequest.SenderId,
            friendRequest.Sender.UserName,
            friendRequest.RecieverId,
            friendRequest.Reciever.UserName,
            friendRequest.SentAt
            );
    }
    public static MessageDto AsMessageDto(this Message message)
    {
        return new MessageDto(
            message.Content,
            message.ConversationId
            );
    }

    public static DisplayMessageDto AsDisplayDto(this Message message)
    {
        return new DisplayMessageDto(
            message.Content,
            message.ConversationId,
            message.SenderUsername
            );
    }
}
