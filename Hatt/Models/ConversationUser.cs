using System.ComponentModel.DataAnnotations.Schema;

namespace Hatt.Models;

public class ConversationUser
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }
    public User User { get; set; } = null!;
    public Conversation Conversation { get; set; } = null!;
}