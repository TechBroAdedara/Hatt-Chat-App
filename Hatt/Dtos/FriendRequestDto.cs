using Hatt.Models;

namespace Hatt.Dtos
{
    public record FriendRequestDisplayDto
    (
        int Id,
        string SenderId,
        string SenderUsername,
        string RecieverId,
        string RecieverUsername,
        DateTime SentAt
    );

    public class FriendRequestResponseDto
    {
        public FriendshipStatus Status { get; set; }
        public ConversationDisplayDto? Conversation { get; set; }
    }

}
