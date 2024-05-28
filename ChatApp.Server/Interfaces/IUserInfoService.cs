using ChatApp.Models;

namespace ChatApp.Server.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo> Authenticate(string username, string password);
    Task<UserInfo> Register(string username, string password, string email);
}