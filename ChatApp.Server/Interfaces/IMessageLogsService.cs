using ChatApp.Models;

namespace ChatApp.Server.Interfaces;

public interface IMessageLogsService
{
    Task<MessageLogs> Logger(MessageLogs messageLogs);
}