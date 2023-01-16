namespace Database.Entities;

public class Message
{
    public Guid Id { get; set; }

    public string Sender { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public Message(string sender, string text)
    {
        Sender = sender;
        Text = text;
    }

    public Message() {}
}