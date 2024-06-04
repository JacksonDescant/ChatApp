using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;


namespace ChatApp.Server.Hubs;


public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message, string room)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message, room);
    }

    private static readonly ConcurrentDictionary<string, List<string>> RoomUsers = new ConcurrentDictionary<string, List<string>>();

    public async Task JoinRoom(string room)
    {
        var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
        RoomUsers.AddOrUpdate(room, new List<string> { username }, (key, list) =>
        {
            list.Add(username);
            return list;
        });
    }
    
    public async Task LeaveRoom(string room)
    {
        var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
        RoomUsers.AddOrUpdate(room, new List<string> { username }, (key, list) =>
        {
            list.Remove(username);
            return list;
        });
    }
    
    public async Task GetRoomUsers(string room)
    {
        var users = RoomUsers.TryGetValue(room, out var list) ? list : new List<string>();
        await Clients.Caller.SendAsync("ReceiveRoomUsers", users);
    }
}