# IQueryProvider

```C#
namespace System.Linq
{
    public interface IQueryProvider
    {
        IQueryable CreateQuery(Expression expression);
        IQueryable<TElement> CreateQuery<TElement>(Expression expression);
        object Execute(Expression expression);
        TResult Execute<TResult>(Expression expression);
    }
}
```

看到这里两组方法的参数，其实大家已经可以知道，Provider负责执行表达式目录树并返回结果。如果是LINQ to SQL的Provider，则它会负责把表达式目录树翻译为T-SQL语句并并传递给数据库服务器，并返回最后的执行的结果；如果是一个Web Service的Provider，则它会负责翻译表达式目录树并调用Web Service，最终返回结果。

这里四个方法其实就两个操作CreateQuery和Execute（分别有泛型和非泛型），CreateQuery方法用于构造一个 IQueryable<T> 对象，该对象可计算指定表达式目录树所表示的查询，返回的结果是一个可枚举的类型，；而Execute执行指定表达式目录树所表示的查询，返回的结果是一个单一值。