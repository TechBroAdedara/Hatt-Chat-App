using System;

namespace Hatt.Models;

public class FriendRequest
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int RecieverId { get; set; }
    public DateTime SentAt { get; set; }
}
