using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("Messages")]
public class Message
{
    [Key]
    public Guid Id { get; set; }

    public string Sender { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
    public DateTime SendingTime { get; set; }

    public Message(string sender, string text)
    {
        Sender = sender;
        Text = text;
        SendingTime = DateTime.Now;
    }

    public Message() {}
}