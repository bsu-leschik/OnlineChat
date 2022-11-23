namespace OnlineChat.Models.ChatCleanerService;

public interface IChatStorageServiceCleaner : IDisposable
{
    public void Start();
    public void Clean();

    public void Stop();
}