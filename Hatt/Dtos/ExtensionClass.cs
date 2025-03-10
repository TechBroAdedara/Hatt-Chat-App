using System;
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
    public static UserDisplayDto ToUserDisplayDto(this User user)
    {
        return new UserDisplayDto(
            user.Firstname,
            user.Lastname,
            user.Email,
            user.Username
        );
    }
}
