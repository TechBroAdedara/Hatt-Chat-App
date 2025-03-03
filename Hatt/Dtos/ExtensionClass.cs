using System;
using Hatt.Models;

namespace Hatt.Dtos;

public static class ExtensionClass
{
    public static ConversationDto AsDto(this Conversation conversation){
        return new ConversationDto(
            conversation.Name,
            conversation.Type,
            conversation.Participants
        );
    }
    public static DisplayUserDto ToDisplayUserDto(this User user)
    {
        return new DisplayUserDto(
            user.Firstname,
            user.Lastname,
            user.Email,
            user.Username
        );
    }
}
