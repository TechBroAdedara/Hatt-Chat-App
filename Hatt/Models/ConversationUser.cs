using System.ComponentModel.DataAnnotations.Schema;

namespace Hatt.Models;

public class ConversationUser
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("ConversationId")]
    public Conversation Conversation { get; set; }
}