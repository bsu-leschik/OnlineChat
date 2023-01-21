using Extensions;

namespace UnitTests;

public static class AsyncEnumerable
{
    public static IAsyncEnumerable<T> Empty<T>()
    {
        return Enumerable.Empty<T>().AsAsyncEnumerable();
    }

    public static IAsyncEnumerable<T> Of<T>(params T[] objects)
    {
        return objects.AsAsyncEnumerable();
    }
}