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
}
