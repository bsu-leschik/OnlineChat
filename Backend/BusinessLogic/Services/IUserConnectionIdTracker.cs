namespace BusinessLogic.Services;

public interface IUserConnectionIdTracker
{
    public void Add(string username, string connectionId);
    public void Remove(string username);
    public string? GetConnectionId(string username);
}

public class UserConnectionIdTracker : IUserConnectionIdTracker
{
    private readonly Dictionary<string, string> _data = new();
    public void Add(string username, string connectionId)
    {
        lock (_data)
        {
            _data[username] = connectionId;
        }
    }

    public void Remove(string username)
    {
        lock (_data)
        {
            _data.Remove(key: username);
        }
    }

    public string? GetConnectionId(string username)
    {
        lock (_data)
        {
            return _data.GetValueOrDefault(key: username);
        }
    }
}