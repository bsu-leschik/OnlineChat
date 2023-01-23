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

    public async static IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var x in enumerable.WithCancellation(cancellationToken))
        {
            if (predicate(x))
            {
                yield return x;
            }
        }
    }

    public async static IAsyncEnumerable<TResult> CastAsync<TValue, TResult>(this IAsyncEnumerable<TValue> enumerable,
        [EnumeratorCancellation] CancellationToken cancellationToken) where TValue : class, TResult 
                                                                      where TResult : class
    {
        await foreach (var x in enumerable.WithCancellation(cancellationToken))
        {
            yield return x as TResult;
        }
    }

    public async static Task<bool> ContainsAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> predicate,
        CancellationToken cancellationToken)
    {
        var result = await enumerable.FirstOrDefaultAsync(predicate, cancellationToken);
        return result is not null;
    }

    public async static IAsyncEnumerable<T> Append<T>(this IAsyncEnumerable<T> a, IAsyncEnumerable<T> b,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var item in a.WithCancellation(cancellationToken))
        {
            yield return item;
        }
        await foreach (var item in b.WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }
}