using ChatApp.Server.Interfaces;
using ChatApp.Models;
using ChatApp.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Services;

public class UserInfoService : IUserInfoService
{
    private readonly UserInfoContext _context;
    
    public UserInfoService(UserInfoContext context)
    {
        _context = context;
    }
    
    public async Task<UserInfo?> Authenticate(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        return user;
    }
    
    public async Task<UserInfo> Register(string username, string password, string email)
    {
        var user = new UserInfo
        {
            Username = username,
            Password = password,
            Email = email
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}