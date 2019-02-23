# IQueryable

LINQ to SQL returns `IQueryable`

If the query happens at other processes or other machine, you should use `IQueryable`

```C#
public interface IQueryable : IEnumerable
{
  Type ElementType { get; }

  Expression Expression { get; }

  IQueryProvider Provider { get; } // tranlate the expression tree

}
```

# IQueryable< T >

```C#
public interface IQueryable<T> : IEnumerable<T>, IQUeryable, IEnumerable
{

}
```

# Extension methods

All standard query methods are defined in static class `System.Linq.Querable` :

![Enuerable overview screenshot](../../images/linq/Queryable_class_snapshot.png "Linq to SQL extension methods")

Below is one of implementation:
```C#
public static class Queryable
{
  public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, 
      Expression<Func<TSource, bool>> predicate)
  {
    if (source == null)
    {
      throw Error.ArgumentNull("source");
    }

    if (predicate == null)
    {
      throw Error.ArgumentNull("predicate");
    }

    return source.Provider.CreateQuery<TSource>(
      Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod())
      .MakeGenericMethod(new Type[] { typeof(TSource) }), 
      new Expression[] { source.Expression, Expression.Quote(predicate) }));
  }
}
```
**Be noted:** It uses the `Provider`.