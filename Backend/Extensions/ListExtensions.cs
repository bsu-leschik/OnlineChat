namespace Extensions;

public static class ListExtensions
{
    public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> pred)
        => list.FirstOrDefault(pred) != null;
}