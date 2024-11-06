using ChatDotNet.Properties.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatDotNet.Properties.Entities;

public class ChatHub : Hub
{
    
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }
    public override Task OnConnectedAsync()
    { 
        SignalRUser user = new SignalRUser() { ConnectionID = Context.ConnectionId };
        _context.SignalRUser.Add(user);
        _context.SaveChanges();

        return base.OnConnectedAsync();
    } 
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var user =  _context.SignalRUser.Where(c => c.ConnectionID == Context.ConnectionId).FirstOrDefault();
        var userGroup = _context.SignalRGroup_SignalRUser.Where(c => c.SignalRUserConnectionID == user.ConnectionID).FirstOrDefault();
        
        if(user != null)
        {
            _context.SignalRUser.Remove(user);
            await _context.SaveChangesAsync();
        }
        if(user != null && userGroup != null)
        {
            await Clients.Group(userGroup.SignalRGroupName).SendAsync("Send",
                $"({Context.ConnectionId}) has leave the group {userGroup.SignalRGroupName}.");
            _context.SignalRGroup_SignalRUser.Remove(userGroup);
            await _context.SaveChangesAsync();
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessage(string user, string message, string group)
    {
        var signalRMessage = new SignalRMessage() { User = user, Message = message, Group = group, Date = DateTime.Now };
        await _context.SignalRMessage.AddAsync(signalRMessage);
        await _context.SaveChangesAsync();
        await Clients.Group(group).SendAsync("ReceiveMessage", user, message, group);
    }
    
    public async Task Log(string message)
    {
        await Clients.All.SendAsync("ReceiveLog", message);
    }
    
    public async Task<string> GetGroup()
    {
        var userGroup = await _context.SignalRGroup_SignalRUser.Where(c => c.SignalRUserConnectionID == Context.ConnectionId).FirstOrDefaultAsync();
        return userGroup != null ? userGroup.SignalRGroupName : "";
    }
    
    public async Task AddToGroup(string group, string name)
    {
        await _context.SignalRGroup_SignalRUser.AddAsync(new SignalRGroup_SignalRUser() { SignalRGroupName = group, SignalRUserConnectionID = Context.ConnectionId });
        await _context.SaveChangesAsync();
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await Clients.Group(group).SendAsync("Send", $"{name} ({Context.ConnectionId}) has joined the group {group}.");
    }
    
    public async Task RemoveFromGroup(string group, string name)
    {
        var userGroup = await _context.SignalRGroup_SignalRUser.Where(c => c.SignalRUserConnectionID == Context.ConnectionId && c.SignalRGroupName == group).FirstOrDefaultAsync();
        if(userGroup != null)
        {
            _context.SignalRGroup_SignalRUser.Remove(userGroup);
            await _context.SaveChangesAsync();
        }
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        await Clients.Group(group).SendAsync("Send", $"{name} ({Context.ConnectionId}) has leave the group {group}.");
    }
    
    public async Task<List<SignalRMessage>?> GetMessages(string group)
    {
        var messages = await _context.SignalRMessage.Where(c => c.Group == group).Take(50).OrderByDescending(c => c.Date).ToListAsync();
        return messages.Count > 0 ? messages : null;
    }
}