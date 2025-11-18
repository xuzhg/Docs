ODL has the 'ReadUntypedAsString' configuration in the 'ODataMessageReaderSettings' (version <=8.x) as:

```C#
public class ODataMessageReaderSettings
{
    public bool ReadUntypedAsString {get;set;}
}
```

If this configuration enabled, the untyped value is read as 'RawString', otherwise, the untyped value is read based on its real value structure, either primitive, structed or array (collection).

For example, for the below odata payload:
```json
{
  "@odata.context":"http://localhost/$metadata#Customers/$entity",
  "Id":42,
  "Name":
  "UndeclaredProperty":12.3
}
```
where, `UndeclaredProperty` is a dynamic property (undelcared in the type of `Customer`).

### ReadUntypedAsString==true
In this case, we can get the `ODataProperty` from the `ODataResource` whose value is an `ODataUntypedValue`

```C#
ODataResource resource = /*Get the resource from reading.*/;
ODataProperty undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredProperty"));
ODataUntypedValue untypedValue = Assert.IsType<ODataUntypedValue>(undelcaredProperty.Value);
Assert.Equal(""12.3"", untypedValue.RawValue);
```
### ReadUntypedAsString==false
In this case, we can get the `ODataProperty` from the `ODataResource` whose value is an `ODataPrimitiveValue`

```C#
ODataResource resource = /*Get the resource from reading.*/;
ODataProperty undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredProperty"));
Assert.Equal(12.3m, undelcaredProperty.Value);
```
