
## 1. ETag on base type property (primitive/enum)

```xml
<EntityType Name="Base">
   <Property Name="LastChanged" Type="Edm.String" />
</EntityType>
<EntityType Name="Sub" BaseType="Base">
   ...
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

## 2. ETag on sub type property (primitive/enum) 

```xml
<EntityType Name="Base">
   ...
</EntityType>
<EntityType Name="Sub" BaseType="Base">
   <Property Name="LastChanged" Type="Edm.String" />
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Bases" EntityType="NS.Base">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>NS.Sub/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

Let's postpone this functionality.

## 3. ETag on Collection of Primitive/Enum

```xml
<EntityType Name="Sub">
   <Property Name="LastChanged" Type="Collection(Edm.String)" />
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Bases" EntityType="NS.Base">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```
Let's postpone this functionality.

## 4. ETag on sub single property of complex

```xml
<ComplexType Name="Address">
   <Property Name="LastChanged" Type="Edm.String" />
</ComplexType>
<EntityType Name="Sub">
   <Property Name="Location" Type="NS.Address" />
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>Location/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

## 5. ETag on sub collection property of complex

```xml
<ComplexType Name="Address">
   <Property Name="LastChanged" Type="Collection(Edm.String)" />
</ComplexType>
<EntityType Name="Sub">
   <Property Name="Location" Type="NS.Address" />
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>Location/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

Let's postpone this functionality.

## 6. ETag on sub single property of containment navigation property type

```xml
<EntityType Name="City">
   <Property Name="LastChanged" Type="Edm.String" />
</EntityType>
<EntityType Name="Sub">
   <NavigationProperty Name="City" Type="NS.Address" Contained="true"/>
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>City/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

## 7. ETag on sub single property of non-containment navigation property type

```xml
<EntityType Name="City">
   <Property Name="LastChanged" Type="Edm.String" />
</EntityType>
<EntityType Name="Sub">
   <NavigationProperty Name="City" Type="NS.Address"/>
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>City/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

## 8. ETag on sub collection property of containment navigation property type

```xml
<EntityType Name="City">
   <Property Name="LastChanged" Type="Collection(Edm.String)" />
</EntityType>
<EntityType Name="Sub">
   <NavigationProperty Name="City" Type="NS.Address" Contained="true"/>
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>City/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```

## 8. ETag on sub collection property of non-containment navigation property type

```xml
<EntityType Name="City">
   <Property Name="LastChanged" Type="Collection(Edm.String)" />
</EntityType>
<EntityType Name="Sub">
   <NavigationProperty Name="City" Type="NS.Address"/>
</EntityType>

<EntityContainer Name="Default">
   <EntitySet Name="Subs" EntityType="NS.Sub">
     <Annotation Term="Org.OData.Core.V1.OptimisticConcurrency">
        <Collection>
            <PropertyPath>City/LastChanged</PropertyPath>
         </Collection>
        </Annotation>
   </EntitySet>
</EntityContainer>
```
