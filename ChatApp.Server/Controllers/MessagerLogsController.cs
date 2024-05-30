using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using ChatApp.Server.Interfaces;

namespace ChatApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagerLogsController : ControllerBase
{
    private readonly IMessageLogsService _messageLogsService;
    
    public MessagerLogsController(IMessageLogsService messageLogsService)
    {
        _messageLogsService = messageLogsService;
    }
    
    [HttpPost("logger")]
    public async Task<ActionResult<MessageLogs>> Logger(MessageLogs messageLogs)
    {
        var log = await _messageLogsService.Logger(messageLogs);
        if (log == null)
        {
            return BadRequest();
        }
        return Ok(log);
    }
}