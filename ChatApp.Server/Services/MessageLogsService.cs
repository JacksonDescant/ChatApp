using ChatApp.Models;
using ChatApp.Server.Interfaces;
using ChatApp.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Services;

public class MessageLogsService : IMessageLogsService
{
    
    private readonly MessageLogsContext _context;
        
    public MessageLogsService(MessageLogsContext context)
    {
        _context = context;
    }
    
    public async Task<MessageLogs> Logger(MessageLogs messageLogs)
    {
        await _context.MessageLogs.AddAsync(messageLogs);
        await _context.SaveChangesAsync();
        return messageLogs;
    }
}