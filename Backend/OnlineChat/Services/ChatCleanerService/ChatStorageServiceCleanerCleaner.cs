using OnlineChat.Models;
using OnlineChat.Models.ChatCleanerService;

namespace OnlineChat.Services.ChatCleanerService;

public class ChatStorageServiceCleanerCleaner : IChatStorageServiceCleaner
{
    private readonly Storage _storage;
    public int TimeoutMs
    {
        get => TimeoutMs;
        set => UpdateTimeout(value); 
    }
    public int MaxEmptyTimeSec { get; set; }

    private Timer? _timer = null;

    public ChatStorageServiceCleanerCleaner(int timeoutMs, Storage storage, int maxEmptyTimeSec)
    {
        TimeoutMs = timeoutMs;
        _storage = storage;
        MaxEmptyTimeSec = maxEmptyTimeSec;
    }

    public async void Start()
    {
        _timer = new Timer(CleanDelegate, null, 0, TimeoutMs);
    }

    public void Clean()
    {
        _storage.RemoveChatrooms(c => c.IsEmpty() && (c.LastEmptyTime - DateTime.Now).TotalSeconds > MaxEmptyTimeSec);
    }

    private void CleanDelegate(object? o)
    {
        Clean();
    }

    public void Stop()
    {
        _timer?.Dispose();
    }

    private void UpdateTimeout(int newValue)
    {
        if (newValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newValue));
        }

        _timer?.Change(0, TimeoutMs);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}