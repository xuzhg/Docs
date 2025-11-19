using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Diagnostics;

namespace ODataUntypedScenarios
{
    internal class ODataUntypedCases
    {
        public static void PlayBasicScenario((IEdmModel, IEdmEntitySet, IEdmEntityType) odata, bool readUntypedAsString)
        {
            string payload = @"{""@odata.context"":""http://localhost/$metadata#Customers/$entity"",
  ""Id"":42,
  ""Name"":""John Joe"",
  ""UndeclaredNum"":12.3,
  ""UndeclaredString"":""12.3""

}";

            ODataHelper.ReadResource(payload, odata.Item1, odata.Item2, odata.Item3, (reader) =>
            {
                switch (reader.State)
                {
                    case ODataReaderState.ResourceEnd:
                        var resource = (ODataResource)reader.Item;
                        PrintResource(resource, readUntypedAsString, "");
                        break;
                }
                
            }, readUntypedAsString);
        }

        public static void PlayEnumScenario((IEdmModel, IEdmEntitySet, IEdmEntityType) odata, bool readUntypedAsString)
        {
            string payload = @"{""@odata.context"":""http://localhost/$metadata#Customers/$entity"",
  ""Id"":42,
  ""Name"":""John Joe"",
  ""UndeclaredString"":""12.3"",
  ""UndeclaredEnum1"":""Red"",
  ""UndeclaredEnum3@odata.type"":""#NS.Color"",
  ""UndeclaredEnum3"":""Red""
}";
            //  ""UndeclaredEnum2"":NS.Color""Red"",
            ODataHelper.ReadResource(payload, odata.Item1, odata.Item2, odata.Item3, (reader) =>
            {
                switch (reader.State)
                {
                    case ODataReaderState.ResourceEnd:
                        var resource = (ODataResource)reader.Item;
                        PrintResource(resource, readUntypedAsString, "");
                        break;
                }

            }, readUntypedAsString);
        }


        public static void PlayResourceScenario((IEdmModel, IEdmEntitySet, IEdmEntityType) odata, bool readUntypedAsString)
        {
            string payload = @"{""@odata.context"":""http://localhost/$metadata#Customers/$entity"",
  ""Id"":42,
  ""Name"":""John Joe"",
  ""UndeclaredComplex"": {
        ""Street"": ""1 Microsoft Way"",
        ""City"": ""Redmond"",
        ""ZipCode"": ""98052""
    }
}";
            //  ""UndeclaredEnum2"":NS.Color""Red"",
            ODataResource top = null;
            ODataResource? complex = null;
            ODataHelper.ReadResource(payload, odata.Item1, odata.Item2, odata.Item3, (reader) =>
            {
                switch (reader.State)
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
                        
                        break;
                }

            }, readUntypedAsString);

            PrintResource(top, readUntypedAsString, "");

            if (complex == null)
            {
                Console.WriteLine("nested resource is null: complex == null");
            }
            else
            {
                PrintResource(complex, readUntypedAsString,"    ");
            }
        }

        public static void PlayResourceSetScenario((IEdmModel, IEdmEntitySet, IEdmEntityType) odata, bool readUntypedAsString)
        {
            string payload = @"{""@odata.context"":""http://localhost/$metadata#Customers/$entity"",
  ""Id"":42,
  ""Name"":""John Joe"",
  ""UndeclaredCollection"": [
    {
        ""Street"": ""1 Microsoft Way"",
        ""City"": ""Redmond"",
        ""ZipCode"": ""98052""
    },
    8,
     ""Simple string"",
     null,
     true
   ]
}";
            //  ""UndeclaredEnum2"":NS.Color""Red"",
            ODataResource? top = null;
            ODataNestedResourceInfo? oDataNestedResourceInfo = null;
            IList<object>? collectionItems = null;
            ODataHelper.ReadResource(payload, odata.Item1, odata.Item2, odata.Item3, (reader) =>
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

            }, readUntypedAsString);

            PrintResource(top, readUntypedAsString, "");

            if (collectionItems == null)
            {
                Console.WriteLine("nested resource set is null: collectionItems == null");
            }
            else
            {
                foreach (var item in collectionItems)
                {
                    if (item is ODataResource resource)
                    {
                        PrintResource(resource, readUntypedAsString, "    ");
                    }
                    else if (item is ODataPrimitiveValue primitiveValue)
                    {
                        Console.WriteLine($"    Primitive item: {primitiveValue.Value} ({primitiveValue.Value?.GetType().Name ?? "null"})");
                    }
                    else
                    {
                        Console.WriteLine($"    Primitive item: {item} ({item?.GetType().Name ?? "null"})");
                    }
                }
            }
        }

        private static void PrintResource(ODataResource? resource, bool readUntypedAsString, string indent)
        {
            if (resource == null)
            {
                Console.WriteLine($"{indent}resource is null");
                return;
            }

            Console.WriteLine($"{indent}Resource of type {resource.TypeName} as readUntypedAsString = {readUntypedAsString}:");
            foreach (var property in resource.Properties.OfType<ODataProperty>())
            {
                if (property.Value is ODataUntypedValue untypedValue)
                {
                    Console.WriteLine($"{indent}  Property {property.Name}: ODataUntypedValue with RawValue = {untypedValue.RawValue} ({untypedValue.RawValue?.GetType().Name ?? "null"})");
                }
                else
                {
                    Console.WriteLine($"{indent}  Property {property.Name}: {property.Value} ({property.Value?.GetType().Name ?? "null"})");
                }
            }
        }
    }
}
