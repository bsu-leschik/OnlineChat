namespace Database;

public abstract class QueryOption<T>
{
    public abstract IQueryable<T> ApplyTo(IQueryable<T> query);
}
