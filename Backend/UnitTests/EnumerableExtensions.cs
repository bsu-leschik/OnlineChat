namespace UnitTests;

public static class EnumerableExtensions
{
    public async static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> enumerable)
    {
        // method is async to return IAsyncEnumerable<T>
        foreach (var item in enumerable)
        {
            yield return item;
        }
    }
}