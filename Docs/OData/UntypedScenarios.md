ODL has the `ReadUntypedAsString` and `EnableUntypedCollections` configuration in the 'ODataMessageReaderSettings' (version <=8.x) as:

```C#
public class ODataMessageReaderSettings
{
    public bool ReadUntypedAsString {get;set;}
    public bool EnableUntypedCollections {get;set;}
}
```

If this configuration enabled, the untyped value is read as 'RawString', otherwise, the untyped value is read based on its real value structure, either primitive, structed or array (collection).

# Primitive

For example, for the below odata payload:
```json
{
  "@odata.context":"http://localhost/$metadata#Customers/$entity",
  "Id":42,
  "Name": "John Joe",
  "UndeclaredNum":12.3,
  "UndeclaredString":"12.3"
}
```
where, `UndeclaredNum` and `UndeclaredString` are dynamic properties (undelcared in the type of `Customer`).

### ReadUntypedAsString==true
In this case, we can get the `ODataProperty` from the `ODataResource` whose value is an `ODataUntypedValue`

```C#
ODataResource resource = /*Get the resource from reading.*/;
ODataProperty undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredNum"));
ODataUntypedValue untypedValue = Assert.IsType<ODataUntypedValue>(undelcaredProperty.Value);
Assert.Equal("12.3", untypedValue.RawValue);

undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredString"));
ODataUntypedValue untypedValue = Assert.IsType<ODataUntypedValue>(undelcaredProperty.Value);
Assert.Equal("\"12.3\"", untypedValue.RawValue);
```
### ReadUntypedAsString==false
In this case, we can get the `ODataProperty` from the `ODataResource` whose value is an `ODataPrimitiveValue`

```C#
ODataResource resource = /*Get the resource from reading.*/;
ODataProperty undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredNum"));
Assert.Equal(12.3m, undelcaredProperty.Value); // it's decimal

undeclaredProperty = Assert.IsType<ODataProperty>(resource.Properties.First(c => c.Name == "UndeclaredString"));
Assert.Equal("12.3", undelcaredProperty.Value); // it's string
```

# Enum

Most of time, it's hard to identify the enum value without the type schema.
```json
{
  "@odata.context":"http://localhost/$metadata#Customers/$entity",
  "Id":42,
  "Name": "John Joe",
  "UndeclaredEnum1": "Red",
  "UndeclaredEnum2@odata.type": "#NS.Color",
  "UndeclaredEnum2":"Red"
}
```

Where, `UndeclaredEnum1` is treated as undeclared `primitive` value, meanwhile `UndeclaredEnum2` is read as `ODataEnumValue` not matter the ReadUntypedAsString is true for false.

# Resource/object

```json
{
  "@odata.context":"http://localhost/$metadata#Customers/$entity",
  "Id":42,
  "Name":"John Joe",
  "UndeclaredComplex": {
        "Street": "1 Microsoft Way",
        "City": "Redmond",
        "ZipCode": "98052"
    }
}
```

We can use the following reading logic:
```C#
while (reader.Read())
{
    case ODataReaderState.ResourceStart:
        var resource = (ODataResource)reader.Item;
        if (top == null)
        {
            top = resource;
        }
        else
        {
            complex = resource;
        }
}
```

### ReadUntypedAsString==true

In this case, `complex==null`, and `UndeclaredComplex` is a property within `top.Properties`, and its value is `ODataUntypedValue`.

` RawValue = "{\"Street\":\"1 Microsoft Way\",\"City\":\"Redmond\",\"ZipCode\":\"98052\"}"`


### ReadUntypedAsString==false
In this case, `complex != null`, and `complex.Properties` contains 3 primitive properties whose types are real value types.

# ResourceSet/array

For the below payload contains mixed item collecion:
```json
{
  "@odata.context":"http://localhost/$metadata#Customers/$entity",
  "Id":42,
  "Name":"John Joe",
  "UndeclaredCollection": [
    {
        "Street": "1 Microsoft Way",
        "City": "Redmond",
        "ZipCode": "98052"
    },
    8,
     "Simple string",
     null,
     true
   ]
}
```

We can use the following codes to read it:
```C#
ODataResource? top = null;
ODataNestedResourceInfo? oDataNestedResourceInfo = null;
IList<object>? collectionItems = null;

while (reader.Read())
{
  switch (reader.State)
  {
      case ODataReaderState.ResourceStart:
          var resource = (ODataResource)reader.Item;
          if (top == null)
          {
              top = resource;
          }
          else if (oDataNestedResourceInfo != null && oDataNestedResourceInfo.Name == "UndeclaredCollection")
          {
              collectionItems!.Add(resource);
          }

          break;

      case ODataReaderState.ResourceSetStart:
          collectionItems = new List<object>();
          break;

      case ODataReaderState.NestedResourceInfoStart:
          oDataNestedResourceInfo = (ODataNestedResourceInfo)reader.Item;
          break;

      case ODataReaderState.Primitive:
          if (oDataNestedResourceInfo != null && oDataNestedResourceInfo.Name == "UndeclaredCollection")
          {
              collectionItems!.Add(reader.Item!);
          }
          break;
  }
}
```

### ReadUntypedAsString==true

In this case, `collectionItems==null`, and `UndeclaredCollection` is a property within `top.Properties`, and its value is `ODataUntypedValue`.

` RawValue = "[{"Street":"1 Microsoft Way","City":"Redmond","ZipCode":"98052"},8,"Simple string",null,true]"`


### ReadUntypedAsString==false
In this case, `collectionItems != null`, and `UndeclaredCollection` contains 5 element items:
```C#
    Resource of type Edm.Untyped as readUntypedAsString = False:
      Property Street: 1 Microsoft Way (String)
      Property City: Redmond (String)
      Property ZipCode: 98052 (String)
    Primitive item: 8 (Int32)
    Primitive item: Simple string (String)
    Primitive item:  (null)
    Primitive item: True (Boolean)
```

For the collection scenario, there's another configuration called `EnableUntypedCollections` that impacts the reading result.

`EnableUntypedCollections` only works with `ReadUntypedAsString==true`.

### ReadUntypedAsString==true && EnableUntypedCollections==false
So, it's same result as above.
`UndeclaredCollection` is a property within `top.Properties`, and its value is `ODataUntypedValue`.

` RawValue = "[{"Street":"1 Microsoft Way","City":"Redmond","ZipCode":"98052"},8,"Simple string",null,true]"`


### ReadUntypedAsString==true && EnableUntypedCollections==true

Now, `UndeclaredCollection` is a property within `top.Properties`, and its value is `ODataCollectionValue`.
Each item in the collection is an `ODataUntypedValue`.
```C#
   Property UndeclaredCollection: ODataCollectionValue of type Edm.Untyped with 5 items:
      Item # 1: ODataUntypedValue with RawValue = {"Street":"1 Microsoft Way","City":"Redmond","ZipCode":"98052"} (String)
      Item # 2: ODataUntypedValue with RawValue = 8 (String)
      Item # 3: ODataUntypedValue with RawValue = "Simple string" (String)
      Item # 4: ODataUntypedValue with RawValue = null (String)
      Item # 5: ODataUntypedValue with RawValue = true (String)
```