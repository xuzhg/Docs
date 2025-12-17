## Reading declared/undeclared nested resource with annotations

```json
{
   "Location@ns.instance1": "value",
   "Location": xxx
}
```

### Design

Make sure the reading process is robust. 
1) For property annotations, read them into the 'ODataNestedResourceInfo.InstanceAnnotations"
2) For resource annotations, read them into the 'ODataResourceBase.InstanceAnnotations"

Specially logic:

```json
{
   "Location@odata.type": "#NS.UsAddress",
   "Location": {
      "@odata.type": "#NS.CnAddress",
      ...
  }
}
```

1) If `Location` is declared, the `@odata.type` property annotation will be read into `ODataNestedResourceInfo.TypeAnnotation.TypeName`.
   The following reading process only checks the `Location` type defined in the model and use that to verify the `@odata.Type` annotation within the resource.
   
3) If `Location` is undeclared, the `@odata.type` property annotation will be read into `ODataNestedResourceInfo.TypeAnnotation.TypeName`, meanwhile
   this type is also used in the following reading process. In this case, it could throw exception since the mismatch between two `@odata.type`.

   ## Writing declared/undeclared nested resource with annotations

   The writing process follow up the odata spec.

   For the non-null resource, only write the instance annotations from `ODataResource.InstanceAnnotations`

   For the null resource, only write the instance annotations from `ODataNestedResourceInfo.InstanceAnnotations` as property annotation.
