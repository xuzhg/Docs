# IEnumerable< T >

LINQ to Objects returns `IEnumerable<T>`.

If the codes are executed in process, you should use `IEnumerable<T>`

```C#
public interface IEnumerable<T> : IEnumerable 
{ 
   IEnumerator<T> GetEnumerator();
}
```

# Extensions methods

All standard query methods are defined in static class `System.Linq.Enumerable` :

![Enuerable overview screenshot](../../images/linq/Enumerable_class_snapshot.png "Linq to Objects extension methods")

# IEnumerable< T > to IQueryable< T >

```C#
public static class Queryable
{
   // ...
   public static IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source);
   public static IQueryable AsQueryable(this IEnumerable source);
}
```