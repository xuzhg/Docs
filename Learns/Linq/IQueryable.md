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