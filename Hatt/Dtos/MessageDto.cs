using System;

namespace Hatt.Dtos;

public record AddMessageDto
(
    int Sender_id,
    DateTime SentAt,
    string Content
);
