## Problem

OneDrive has scenarios where a segment representing a drive path needs to be inserted within a URL to identify a target drive item. The drive-path-segment represents the driveItem referenced by the drive path navigated from the driveItem represented by the previous segment.
They use a special syntax to escape the segment from being processed as part of an OData URL and use ":/" and ":" as the separator to identify the relative path in the URL. 

See example at: https://docs.microsoft.com/en-us/graph/api/driveitem-list-children?view=graph-rest-1.0

![Category overview screenshot](../../Images/odata/onedrivepattern.png "ODL OneDrive Uri pattern")

The problem is that ` :/ ... : ` in the path segment is a non odata standard approach.

## Solution

OData introduces a new term which can be applied to bound functions to annotate them as escaped segments.
See the implementation at: https://github.com/OData/odata.net/blob/master/src/Microsoft.OData.Edm/Vocabularies/CommunityVocabularies.xml#L8-L10

```xml
<Term AppliesTo="Function" Type="Core.Tag" Name="UrlEscapeFunction">
  <Annotation Term="Core.Description" String="Annotates a function to be substituted for a colon-escaped segment in a Url path"/>
</Term>
```

The usage is same as other annotation, below is an example:
```xml
<?xml version="1.0" encoding="utf-16"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
 <edmx:DataServices>
   <Schema Namespace="NS" xmlns="http://docs.oasis-open.org/odata/ns/edm">
     <EntityType Name="Entity">
       <Key>
         <PropertyRef Name="Id" /> 
       </Key> 
       <Property Name="Id" Type="Edm.Int32" Nullable="false" /> 
     </EntityType> 
     <Function Name="Function" IsBound="true"> 
       <Parameter Name="entity" Type="NS.Entity" /> 
       <Parameter Name="path" Type="Edm.String" /> 
       <ReturnType Type="Edm.Int32" /> 
       <Annotation Term="Org.OData.Community.V1.UrlEscapeFunction" Bool="true" /> 
     </Function> 
   </Schema> 
 </edmx:DataServices> 
</edmx:Edmx>
```

## Validation
So far, we create the following validation for a bound function which has been annotated with `UriEscapeFunction` term:

1. For one binding type, there is at most one escape function for composable binding function, see [here](https://github.com/OData/odata.net/pull/1400/files#diff-cb9c6a124b96938f3493f2aa2e7eb4caR1017).
2. For one binding type, there is at most one escape function for un-composable binding function, see [here](https://github.com/OData/odata.net/pull/1400/files#diff-cb9c6a124b96938f3493f2aa2e7eb4caR1017).
3. Escapable function should have and only have two parameters, the non-binding parameter type should be "Edm.String", see [here](https://github.com/OData/odata.net/blob/master/src/Microsoft.OData.Edm/Validation/ValidationRules.cs#L1753).

