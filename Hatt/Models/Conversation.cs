namespace Hatt.Models;

public enum ConversationType
{
    Private,
    Public
}
public class Conversation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ConversationType Type { get; set; }
    public int Participants { get; set; }
    public List<Message?> Messages { get; set; } = [];


}