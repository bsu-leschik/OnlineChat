using System.Runtime.CompilerServices;

namespace Extensions;

public static class AsyncEnumerableExtensions
{
    public async static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable,
        CancellationToken cancellationToken)
    {
        var result = new List<T>();
        await foreach (var element in enumerable.WithCancellation(cancellationToken))
        {
            result.Add(element);       
        }
        return result;
    }

    public async static IAsyncEnumerable<TR> SelectAsync<T, TR>(this IAsyncEnumerable<T> enumerable, Func<T, TR> func,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var element in enumerable.WithCancellation(cancellationToken))
        {
            yield return func(element);
        }
    }

    public async static Task<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> predicate,
        CancellationToken cancellationToken)
    {
        await foreach (var element in enumerable.WithCancellation(cancellationToken))
        {
            if (predicate(element))
            {
                return element;
            }
        }
        return default;
    }
}