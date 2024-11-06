using System.ComponentModel.DataAnnotations;

namespace ChatDotNet.Properties.Entities;

public class SignalRUser()
{
    [Key]
    public string ConnectionID { get; set; }
}