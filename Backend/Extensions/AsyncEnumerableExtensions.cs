using System.Runtime.CompilerServices;

namespace Extensions;

public static class AsyncEnumerableExtensions
{
    public async static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable,
        CancellationToken cancellationToken)
    {
        var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
        var result = new List<T>();
        while (await enumerator.MoveNextAsync())
        {
            result.Add(enumerator.Current);
        }
        await enumerator.DisposeAsync();
        return result;
    }

    public async static IAsyncEnumerable<TR> SelectAsync<T, TR>(this IAsyncEnumerable<T> enumerable, Func<T, TR> func,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync())
        {
            yield return func(enumerator.Current);
        }
        await enumerator.DisposeAsync();
    }

    public async static Task<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> predicate,
        CancellationToken cancellationToken)
    {
        var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync())
        {
            if (!predicate(enumerator.Current)) continue;
            await enumerator.DisposeAsync();
            return enumerator.Current;
        }
        await enumerator.DisposeAsync();
        return default;
    }
}