namespace Hatt.Models;

public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty!;
    public string Lastname { get; set; } = string.Empty!;
    public string Email { get; set; } = string.Empty!;
    public string Username { get; set; } = string.Empty!;
    public string HashedPassword { get; set; } = string.Empty!;

    public List<ConversationUser> ConversationUsers { get; set; } = [];
}

