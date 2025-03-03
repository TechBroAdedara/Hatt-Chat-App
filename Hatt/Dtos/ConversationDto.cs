using System;
using System.ComponentModel.DataAnnotations;
using Hatt.Models;

namespace Hatt.Dtos;

public record ConversationDto (
    [Required]
    string Name,
    ConversationType Type,
    int Participants
);

public record UpdateConversationDto(
    int Id, string Name
);