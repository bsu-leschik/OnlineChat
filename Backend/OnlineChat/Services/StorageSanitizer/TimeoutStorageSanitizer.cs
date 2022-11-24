namespace OnlineChat.Services.StorageSanitizer;

public class TimeoutStorageSanitizer : IStorageSanitizer, IDisposable
{
    private readonly Storage _storage;
    private readonly PeriodicTimer _timer;
    private readonly TimeSpan _maxEmptyTime;

    public TimeoutStorageSanitizer(Storage storage, TimeSpan sanitizeInterval, TimeSpan maxEmptyTime)
    {
        _storage = storage;
        _maxEmptyTime = maxEmptyTime;
        _timer = new PeriodicTimer(sanitizeInterval);
    }

    public void Sanitize()
    {
        _storage.RemoveChatrooms(room => 
            room.IsEmpty() && 
            (DateTime.Now - room.LastEmptyTime > _maxEmptyTime));
    }

    public async void StartSanitizing()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            Sanitize();
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}