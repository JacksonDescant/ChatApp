namespace ChatApp.Models;

public class AuthenticatedUser
{
    public string Email { get; set; } = string.Empty;
    public Dictionary<string, string> Claims { get; set; } = [];
}