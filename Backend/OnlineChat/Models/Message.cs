namespace OnlineChat.Models;

public class Message
{
    public Message(string sender, string text)
    {
        Sender = sender;
        Text = text;
    }

    public string Sender { get; set; }
    public string Text { get; set; }
}