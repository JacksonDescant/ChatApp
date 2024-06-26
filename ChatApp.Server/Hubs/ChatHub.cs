using System.Collections.Concurrent;
using System.Security.Claims;
using ChatApp.Server.Data;
using ChatApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace ChatApp.Server.Hubs;


public class ChatHub : Hub
{

    private readonly IChatRoomUserService _chatRoomUserService;
    public ChatHub([FromServices] IChatRoomUserService chatRoomUserService)
    {
        _chatRoomUserService = chatRoomUserService;
    }
    
    public async Task SendMessage(string user, string message, string room)
    {
        await Clients.Group(room).SendAsync("ReceiveMessage", user, message, room);
    }
    
    public async Task JoinRoom(string room, string username)
    {
        var test = Context;
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await _chatRoomUserService.AddUserToGroup(room, username);
        await Clients.Group(room).SendAsync("ReceiveMessage", "", $"{username} has joined the chat", room);
        var users = await _chatRoomUserService.GetUsersInGroup(room);
        await Clients.Group(room).SendAsync("UpdateUserList", users);
    }
    
    public async Task LeaveRoom(string room, string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        await _chatRoomUserService.RemoveUserFromGroup(room, username);
        await Clients.Group(room).SendAsync("ReceiveMessage", "", $"{username} has left the chat", room);
        var users = await _chatRoomUserService.GetUsersInGroup(room);
        await Clients.Group(room).SendAsync("UpdateUserList", users);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string user = Context.User.Identity.Name;
        
        await _chatRoomUserService.RemoveUserFromAllGroups(user);


        await base.OnDisconnectedAsync(exception);
    }
}