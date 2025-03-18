using Microsoft.AspNetCore.Identity;

namespace Hatt.Models;

public class User: IdentityUser
{
    public string Firstname { get; set; } = string.Empty!;
    public string Lastname { get; set; } = string.Empty!;

    public List<ConversationUser> ConversationUsers { get; set; } = [];
}

