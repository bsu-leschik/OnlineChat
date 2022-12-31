using BusinessLogic.Services;
using Database;
using Database.Entities;

namespace BusinessLogic.Extensions;

public static class StorageServiceExtensions
{
    public async static Task<bool> Contains(this IStorageService storageService, Func<User, bool> pred, CancellationToken cancellationToken)
    {
        return await storageService.GetUser(pred, cancellationToken) != null;
    }
}