using ChatApp.Models;

namespace ChatApp.Server.Interfaces;

public interface IChatRoomService
{
    Task<List<ChatRoom>> GetChatRooms();
    Task<ChatRoom> GetChatRoom(int id);
    
}