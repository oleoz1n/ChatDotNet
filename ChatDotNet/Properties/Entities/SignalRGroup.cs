using System.ComponentModel.DataAnnotations;

namespace ChatDotNet.Properties.Entities;


public class SignalRGroup
{
    [Key]
    public string Name { get; set; }
    public List<string> Users { get; set; }

    public SignalRGroup()
    {
        Users = new List<string>();
    }
}