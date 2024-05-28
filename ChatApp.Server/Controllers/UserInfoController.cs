using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using ChatApp.Server.Interfaces;

namespace ChatApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly IUserInfoService _userInfoService;
    
    public UserInfoController(IUserInfoService userInfoService)
    {
        _userInfoService = userInfoService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> Get()
    {
        return Ok("SUP");
    }
    
    [HttpPost("authenticate")]
    public async Task<ActionResult<UserInfo>> Authenticate([FromBody] UserInfo userInfo)
    {
        var user = await _userInfoService.Authenticate(userInfo.Username, userInfo.Password);
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