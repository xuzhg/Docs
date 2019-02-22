# IQueryable< T >

```C#
public interface IQueryable:IEnumerable
{
  Type ElementType { get; }

  Expression Expression { get; }

  IQueryProvider Provider { get; }

}
```
