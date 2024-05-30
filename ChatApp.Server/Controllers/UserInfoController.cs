using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using ChatApp.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Server.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly IUserInfoService _userInfoService;
    
    public UserInfoController(IUserInfoService userInfoService)
    {
        _userInfoService = userInfoService;
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<UserInfo>> Authenticate(UserInfo userInfo)
    {
        var user = await _userInfoService.Authenticate(userInfo);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(user);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserInfo>> Register(RegisterModel registerModel)
    {
        var user = await _userInfoService.Register(registerModel.Username, registerModel.Password, registerModel.Email);
        if (user == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Authenticate), new { username = user.Username, password = user.Password }, user);
    }
}