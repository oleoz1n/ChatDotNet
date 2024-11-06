using ChatDotNet.Properties.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatDotNet.Properties.Data;

// implement the DbContext class
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<SignalRUser> SignalRUser { get; set; }
    public DbSet<SignalRGroup> SignalRGroup { get; set; }
    public DbSet<SignalRGroup_SignalRUser> SignalRGroup_SignalRUser { get; set; }
    public DbSet<SignalRMessage> SignalRMessage { get; set; }
}