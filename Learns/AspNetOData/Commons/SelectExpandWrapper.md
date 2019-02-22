# SelectExpandWrapper

Wrapper the $select & $expand clause.

```C#
internal abstract class SelectExpandWrapper : IEdmEntityObject, ISelectExpandWrapper
{
  public PropertyContainer Container { get; set; }
  public string ModelID { get; set; }  // EF doesn't let you inject non-primitive constance value.
  public object UntypedInstance { get; set; }
  public string InstanceType { get; set; }
  public bool UseInstanceForProperties { get; set; }
}
```

## interface

```C#
IEdmEntityObject
```

```C#
// Represents the result of a $select and $expand query operation.
ISelectExpandWrapper
```
