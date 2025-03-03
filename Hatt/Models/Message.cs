using System.Text.Json.Serialization;

namespace Hatt.Models;

public class Message
{
    public int Id { get; set; }
    public int Sender_id { get; set; }
    public DateTime SentAt { get; set; }
    public string Content { get; set; } = string.Empty;
    public int ConversationId { get; set; }
    [JsonIgnore]
    public Conversation? Conversation { get; set; }

}
