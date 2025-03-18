using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hatt.Models;

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Declined,
    Blocked
}
public enum FriendshipStatusMotive
{
    Accept,
    Decline,
    Block
}
public class FriendRequest
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public FriendshipStatus Status { get; set; } //Status of friendrequest, of type Status
    public DateTime SentAt { get; set; }

    public string SenderId { get; set; }
    public User Sender { get; set; } //Navigation Property: Sender of friend request, of type User

    public string RecieverId { get; set; }
    public User Reciever { get; set; } //Navigation Property: Reciever of friend request, also of type User


}
