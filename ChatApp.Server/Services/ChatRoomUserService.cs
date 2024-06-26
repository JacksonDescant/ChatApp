using ChatApp.Server.Interfaces;

namespace ChatApp.Server.Services;

public class ChatRoomUserService : IChatRoomUserService
{
    public List<Tuple<string, string>> UsersInGroup = new List<Tuple<string,string>>();
    
    public ChatRoomUserService(){}

    public Task AddUserToGroup(string groupName, string user)
    {
        UsersInGroup.Add(new Tuple<string, string>(groupName, user));
        return Task.CompletedTask;
    }
    
    public Task RemoveUserFromGroup(string groupName, string user)
    {
        UsersInGroup.RemoveAll(x => x.Item1 == groupName && x.Item2 == user);
        return Task.CompletedTask;
    }
    
    public Task RemoveUserFromAllGroups(string user)
    {
        UsersInGroup.RemoveAll(x=> x.Item2 == user);
        return Task.CompletedTask;
    }

    public Task<List<Tuple<string,string>>> GetUsersInGroup(string groupName)
    {
        var users = UsersInGroup.Where(x => x.Item1 == groupName).ToList();
        return Task.FromResult(users);
    }
    
    
}