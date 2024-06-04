using ChatApp.Models;
using ChatApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatRoomController : ControllerBase
{
    private readonly IChatRoomService _chatRoomService;

    public ChatRoomController(IChatRoomService chatRoomService)
    {
        _chatRoomService = chatRoomService;
    }

    [HttpGet("GetChatRooms")]
    public async Task<ActionResult<List<ChatRoom>>> GetChatRooms()
    {
        try
        {
            return await _chatRoomService.GetChatRooms();
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("GetChatRoom/{Id}")]
    public async Task<ActionResult<ChatRoom>> GetChatRoom(int id)
    {
        var chatRoom = await _chatRoomService.GetChatRoom(id);
        if (chatRoom == null)
        {
            return NotFound();
        }
        return chatRoom;
    }
}