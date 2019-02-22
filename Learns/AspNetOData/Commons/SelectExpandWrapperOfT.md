## SelectExpandWrapper < T >

Represents a container class that contains properties that are either selected or expanded using $select and $expand.
```C#
internal class SelectExpandWrapper<TElement> : SelectExpandWrapper
{
  public TElement Instance {get;set;}
}
```

## Derived types

```C#
private class SelectAllAndExpand<TEntity> : SelectExpandWrapper<TEntity>
private class SelectAll<TEntity> : SelectExpandWrapper<TEntity>
private class SelectSomeAndInheritance<TEntity> : SelectExpandWrapper<TEntity>
private class SelectSome<TEntity> : SelectAllAndExpand<TEntity>
```

Entityframework requires that the two different type initializers for a given type in the same query have the same set of properties in the same order.

A `~/People?$select=Name&$expand=Friend` results in a select expression that has two `SelectExpandWrapper<Person>` expressions,
one for the root level person and the second for the expanded Friend person.
The first wrapper has the Container property set (contains Name and Friend values) where as the second wrapper
has the Instance property set as it contains all the properties of the expanded person.
