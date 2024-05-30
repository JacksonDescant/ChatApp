using ChatApp.Models;

namespace ChatApp.Server.Interfaces;

public interface IUserInfoService
{
    Task<string?> Authenticate(UserInfo userInfo);
    Task<UserInfo> Register(string username, string password, string email);
}