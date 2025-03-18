using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hatt.Models;

public class Message
{
    public int Id { get; set; }
    public string SenderUsername { get; set; } = string.Empty!;
    public DateTime SentAt { get; set; }
    public string Content { get; set; } = string.Empty;

    [ForeignKey("Conversation")]
    public int ConversationId { get; set; }
    public Conversation? Conversation { get; set; }

}
