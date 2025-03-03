namespace Hatt.Models;

public class ConversationUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.Now;
    public DateTime? LeftAt { get; set; }

    public User User { get; set; } = null!;
    public Conversation Conversation { get; set; } = null!;
}