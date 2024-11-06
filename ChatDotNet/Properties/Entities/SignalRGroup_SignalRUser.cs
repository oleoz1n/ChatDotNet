using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatDotNet.Properties.Entities;

public class SignalRGroup_SignalRUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SignalRGroup_SignalRUserID { get; set; }
    public string SignalRGroupName { get; set; }
    public SignalRGroup SignalRGroup { get; set; }
    public string SignalRUserConnectionID { get; set; }
    public SignalRUser SignalRUser { get; set; }
}