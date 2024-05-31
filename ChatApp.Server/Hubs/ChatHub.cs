using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message, string room)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message, room);
    }

    public async Task SetBanner(string room, string bannerUrl)
    {
        await Clients.All.SendAsync("ReceiveBanner", room, bannerUrl);
    }
}