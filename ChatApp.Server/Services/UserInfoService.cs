using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatApp.Server.Interfaces;
using ChatApp.Models;
using ChatApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Server.Services;

public class UserInfoService : IUserInfoService
{
    private readonly UserInfoContext _context;
    
    public UserInfoService(UserInfoContext context)
    {
        _context = context;
    }
    
    public async Task<string?> Authenticate(UserInfo userInfo)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userInfo.Username && u.Password == userInfo.Password);
        if(user == null)
        {
            return null;
        }
        var token = GenerateJwtToken(user);
        return token;
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
    
    private string GenerateJwtToken(UserInfo user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("super secret key asdasdfasdfasdfasdfhasdghjgkjhgasmdfjas");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        tokenDescriptor.Claims = new Dictionary<string, object>();
        tokenDescriptor.Claims.Add("username", user.Username);
        tokenDescriptor.Claims.Add("email", user.Email);

        try
        {
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return "";
        }

    }
}