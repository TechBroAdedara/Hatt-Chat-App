namespace Hatt.Dtos
{
    public record ConversationUserDto
    (
        string UserId,
        Guid ConversationId,
        string Role
    );
}
