using System;

namespace Hatt.Dtos;

public record AddMessageDto
(
    string Content,
    int ConversationId
);

