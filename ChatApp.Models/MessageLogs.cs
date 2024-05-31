namespace ChatApp.Models;

public class MessageLogs
{
    public int Id { get; set; }
    public string Room { get; set; }
    public string Username { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }
}