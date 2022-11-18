namespace OnlineChat;

public class UsernameDictionary
{
    private readonly Dictionary<string, string> _usernames = new();
    public void Add(object key, object? value)
    {
        ((System.Collections.IDictionary) _usernames).Add(key, value);
    }

    public bool Contains(object key)
    {
        return ((System.Collections.IDictionary) _usernames).Contains(key);
    }

    public void Remove(object key)
    {
        ((System.Collections.IDictionary) _usernames).Remove(key);
    }

    public void Add(string key, string value)
    {
        _usernames.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _usernames.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _usernames.Remove(key);
    }

    public int Count => _usernames.Count;

    public string Get(string key)
    {
        return _usernames[key];
    }

    public void Set(string key, string value)
    {
        _usernames[key] = value;
    }
}