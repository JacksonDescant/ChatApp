namespace ChatApp.Client.Authorization;

public interface IAccountManagement
{
    public Task LoginAsync(string username, string password);
    public Task LogoutAsync();
    public Task RegisterAsync(string email, string password);

    public Task<bool> CheckAuthenticatedAsync();
}