# IEnumerable< T >

LINQ to Objects returns `IEnumerable<T>`.

```C#
public interface IEnumerable<T> : IEnumerable 
{ 
   IEnumerator<T> GetEnumerator();
}
```
