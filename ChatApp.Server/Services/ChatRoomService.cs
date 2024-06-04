using Azure.Core;
using ChatApp.Models;
using ChatApp.Server.Data;
using ChatApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace ChatApp.Server.Services;

public class ChatRoomService : IChatRoomService
{
    private readonly ChatRoomContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public ChatRoomService(ChatRoomContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetServerUrl()
    {
        var request = _httpContextAccessor.HttpContext.Request;
        return $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
    
    public async Task<List<ChatRoom>> GetChatRooms()
    {
        var rooms = await _context.ChatRooms.ToListAsync();
        rooms.ForEach(room =>
        {
            room.Banner = $"{GetServerUrl()}/images/{room.Banner}";
        });
        return rooms;
    }
    
    public async Task<ChatRoom> GetChatRoom(int id)
    {
        var room = await _context.ChatRooms.FindAsync(id);
        if (room != null)
        {
            room.Banner = $"{GetServerUrl()}/images/{room.Banner}";
        }
        return room;
    }
}