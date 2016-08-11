
## Single property multiple path binding

# CLR models

## Entity types
```C#
public class Cusomter
{
  public int Id { get; set; }
  
  public Address Location { get; set; }
  
  public Geography Geo { get; set;}
}

public class City
{
  public int Id { get; set; }
}
```

## Complex types
```C#
public class Address
{
  public City City1 { get; set; }
  public City City2 { get; set; }
  
  public string Street { get; set; }
}

public class Geography
{
  public Address GeoAddress { get; set; }
}
```

# Add navigation property binding:

```C#

customers.HasRequiredPath(c => c.Location).HasRequiredBinding(a => a.City1, "Cities1");

customers.HasRequiredPath(c => c.Location).HasRequiredBinding(a => a.City2, "Cities2");

customers.HasRequiredPath(c => c.Geo)
  .HasOptionalPath(g => g.GeoAddress)
  .HasRequiredBinding(a => a.City1, "Cities1");

customers.HasRequiredPath(c => c.Geo)
  .HasOptionalPath(g => g.GeoAddress)
  .HasRequiredBinding(a => a.City2, "SpecialCities2");
  
```

So, we can get the following target binding:

```xml

<Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
  <EntityContainer Name="Container">
    <EntitySet Name="Customers" EntityType="System.Web.OData.Builder.Cusomter">
      <NavigationPropertyBinding Path="Location/City1" Target="Cities1" />
      <NavigationPropertyBinding Path="Geo/GeoAddress/City1" Target="Cities1" />
      <NavigationPropertyBinding Path="Location/City2" Target="Cities2" />
      <NavigationPropertyBinding Path="Geo/GeoAddress/City2" Target="SpecialCities2" />
    </EntitySet>
    <EntitySet Name="Cities1" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="Cities2" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="SpecialCities2" EntityType="System.Web.OData.Builder.City" />
  </EntityContainer>
</Schema>
```
