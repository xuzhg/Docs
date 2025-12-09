The content of a single nested resource could be:
1. Valid entity
```json
{
  "@odata.context":"http://www.sampletest.com/$metadata#Customers/$entity",
   "Id":42,
   "Location": {
       "@odata.type":"NS.UndefAddress",
       "@NS.unknownName":"unknown value",
       ...
   }
}
```

2. 'null' value

```json
{
  "@odata.context":"http://www.sampletest.com/$metadata#Customers/$entity",
   "Id":42,
   "Location@NS.unknownName":"unknown value",
   "Location@odata.type":"NS.UndefAddress",
   "Location":null
}
```

3. without content
```json
{
  "@odata.context":"http://www.sampletest.com/$metadata#Customers/$entity",
   "Id":42,
   "Location@NS.unknownName":"unknown value",
   "Location@odata.type":"NS.UndefAddress"
}
```

The current ODL reading process has different output:

```C#
ODataResource topLevel = null;
ODataResource location = null;
bool readLocation = false;
ODataNestedResourceInfo nestedInfo = null;
ODataPropertyInfo propertyInfo = null;
using (var msgReader = new ODataMessageReader((IODataResponseMessage)message, readerSettings, edmModel))
{
    var reader = msgReader.CreateODataResourceReader(entitySet, entityType);
    while (reader.Read())
    {
         if (reader.State == ODataReaderState.ResourceStart)
         {
             if (entry == null)
              {
                  entry = (ODataResource)reader.Item;
              }
              else
              {
                  readLocation = true;
                  location = (ODataResource)reader.Item;
              }
         }
         else if (reader.State == ODataReaderState.NestedResourceInfoStart)
         {
              nestedInfo = (reader.Item as ODataNestedResourceInfo);
         }
         else if (reader.State == ODataReaderState.NestedProperty)
         {
             propertyInfo = (ODataPropertyInfo)(reader.Item);
         }
    }
}
```
So, here's the output for different JSON payload:

1. With content

```C#
topLevel != null;
location != null && readLocation == true; // can retrieve the annotation from `Location`
nestedInfo != null;
propertyInfo == null;
```

2. 'null' value
```C#
topLevel != null;
location == null && readLocation == true; // CANNOT retrieve the annotation from `Location` since it's value is null.
nestedInfo != null;
propertyInfo == null;
```

3. without content

- If `location` is undeclared (dynamic) property, throw exception.
- If `location` is declared property, we have the following:

```C#
topLevel != null;
location == null && readLocation == false; // without hit the second `ODataReaderState.ResourceStart`.
nestedInfo != null;
propertyInfo != null; // can retrieve the annotation from `nested property info`
```

# Problems
1. For `null` null, we'd figure out a solution to retrieve the annotations
2. For without content, we'd fix the exception if the property is undelcared.
