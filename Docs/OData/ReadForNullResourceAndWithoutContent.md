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
1. For `null` value, we'd figure out a solution to retrieve the annotations
2. For without content, we'd fix the exception if the property is undelcared.


## Designs for `null` value
Let's see the writing process first here:
https://github.com/OData/odata.net/blob/main/src/Microsoft.OData.Core/Json/ODataJsonWriter.cs#L371-L379
```C#
ODataNestedResourceInfo parentNavLink = this.ParentNestedResourceInfo;
if (parentNavLink != null)
{
    // For a null value, write the type as a property annotation
    if (resource == null)
    {
        if (parentNavLink.TypeAnnotation != null && parentNavLink.TypeAnnotation.TypeName != null)
        {
            this.odataAnnotationWriter.WriteODataTypePropertyAnnotation(parentNavLink.Name, parentNavLink.TypeAnnotation.TypeName);
        }

        this.instanceAnnotationWriter.WriteInstanceAnnotations(parentNavLink.GetInstanceAnnotations(), parentNavLink.Name);
    }

    // Write the property name of an expanded navigation property to start the value.
    this.jsonWriter.WriteName(parentNavLink.Name);
}

if (resource == null)
{
    this.jsonWriter.WriteValue((string)null);
    return;
}
```

It says, if the resource is 'null', let's write the annotation from 'NestedResourceInfo` by calling `GetInstanceAnnotations()`. 
That's good. It's clear that:
1) ODataNestedResourceInfo can contain the annotations for the nested resource.
2) `GetInstanceAnnotations()` and `SetInstanceAnnotations()` are internal, so there's no public way to specify the annotations on ODataNestedResourceInfo.

In this case, I think we can expose the functionalites to add/retrieve annotations on `ODataNestedResourceInfo`.

In reading, the instance annotations as property annotation (aka, "propertyname@...": ...) are saved into 'ODataNestedResourceInfo` except the control metadatas (aka, @odata.type).
the instance annotations as annotation within the resource (aka, "proeprty": { "@...": ... }) are saved into 'ODataResource' directly, this keeps back-compatible.

In writing, any annotations on `ODataNestedResourceInfo` are written as property annotation, otherwise, the annotations on `ODataResource` are written as resource annotations.


## Designs for `null` value

For the undeclared nested resource without content, let's output the 'ODataReaderState.ODataNestedProperty'. This keeps consistent between declared and undeclared.
