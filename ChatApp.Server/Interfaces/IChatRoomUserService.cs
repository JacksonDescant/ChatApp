namespace ChatApp.Server.Interfaces;

public interface IChatRoomUserService 
{
    public Task AddUserToGroup(string groupName, string user);
    public Task RemoveUserFromGroup(string groupName, string user);
    public Task RemoveUserFromAllGroups(string user);
    public Task<List<Tuple<string,string>>> GetUsersInGroup(string groupName);

}