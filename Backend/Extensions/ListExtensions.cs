namespace Extensions;

public static class ListExtensions
{
    /// <summary>
    /// Checks if the collection contains element satisfying the predicate
    /// </summary>
    /// <param name="list"></param>
    /// <param name="pred"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>true, if collection contains such element, otherwise false</returns>
    public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> pred)
        => list.FirstOrDefault(pred) != null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool EqualAsSets<T>(List<T> a, List<T> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }

        return a.All(b.Contains) && b.All(a.Contains);
    }
}