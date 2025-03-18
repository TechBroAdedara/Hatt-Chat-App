using Microsoft.AspNetCore.Identity;

namespace Hatt.Models;

public class User: IdentityUser
{
    public string Firstname { get; set; } = string.Empty!;
    public string Lastname { get; set; } = string.Empty!;

    public List<ConversationUser> ConversationUsers { get; set; } = [];
    
    //Requests sent by user
    public List<FriendRequest> SentFriendRequests { get; set; } = [];

    //Requests recieved by user
    public List<FriendRequest> RecievedFriendRequests { get; set; } = [];

}

