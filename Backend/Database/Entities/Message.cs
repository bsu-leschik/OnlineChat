namespace Database.Entities;

public class Message
{
    public Guid Id { get; set; }

    public string Sender { get; set; }

    public string Text { get; set; }

    public Message(string sender, string text)
    {
        Id = Guid.NewGuid();
        Sender = sender;
        Text = text;
    }

    public Message() {}
}