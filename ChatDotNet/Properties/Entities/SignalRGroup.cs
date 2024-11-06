using System.ComponentModel.DataAnnotations;

namespace ChatDotNet.Properties.Entities;


public class SignalRGroup
{
    [Key]
    public string Name { get; set; }
    public List<SignalRUser> Users { get; set; }
    public List<SignalRMessage> Messages { get; set; } 

}