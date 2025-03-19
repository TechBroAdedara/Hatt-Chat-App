using System;

namespace Hatt.Dtos;

public record MessageDto
(
    string Content,
    Guid ConversationId
);

public record DisplayMessageDto
    (
    string Content,
    Guid ConversationId,
    string SenderUsername
    );