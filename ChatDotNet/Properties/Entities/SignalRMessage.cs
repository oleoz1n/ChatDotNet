using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatDotNet.Properties.Entities;

public class SignalRMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string User { get; set; }
    public string Message { get; set; }
    public string Group { get; set; }
    public DateTime Date { get; set; }
}