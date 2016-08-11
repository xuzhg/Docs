## Collection property multiple path binding

# CLR models

## Entity types
```C#
public class Cusomter
{
  public int Id { get; set; }
  
  public IList<Address> Locations { get; set; }
  
  public IList<Geography> Geoes { get; set;}
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
  
  public IList<City> Cities { get; set; }
}

public class Geography
{
  public Address GeoAddress { get; set; }
  
  public IList<Address> GeoAddresses { get; set; }
}
```

# add navigation property binding:

```C#
customers.HasManyPath(c => c.Locations)
    .HasRequiredBinding(a => a.City1, "Cities1");

customers.HasManyPath(c => c.Locations)
    .HasManyBinding(a => a.Cities, "Cities2");

customers.HasManyPath(c => c.Geoes)
    .HasOptionalPath(g => g.GeoAddress)
    .HasRequiredBinding(a => a.City1, "Cities1");

customers.HasRequiredPath(c => c.Geo)
    .HasOptionalPath(g => g.GeoAddress)
    .HasManyBinding(a => a.Cities, "Cities2");

customers.HasManyPath(c => c.Geoes)
    .HasManyPath(g => g.GeoAddresses)
    .HasRequiredBinding(a => a.City1, "Cities1");

customers.HasRequiredPath(c => c.Geo)
    .HasManyPath(g => g.GeoAddresses)
    .HasManyBinding(a => a.Cities, "Cities2");
```

We can get the following target binding:

```xml
<Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
  <EntityContainer Name="Container">
    <EntitySet Name="Customers" EntityType="System.Web.OData.Builder.Cusomter">
      <NavigationPropertyBinding Path="Locations/City1" Target="Cities1" />
      <NavigationPropertyBinding Path="Geoes/GeoAddress/City1" Target="Cities1" />
      <NavigationPropertyBinding Path="Geoes/GeoAddresses/City1" Target="Cities1" />
      <NavigationPropertyBinding Path="Locations/Cities" Target="Cities2" />
      <NavigationPropertyBinding Path="Geo/GeoAddress/Cities" Target="Cities2" />
      <NavigationPropertyBinding Path="Geo/GeoAddresses/Cities" Target="Cities2" />
    </EntitySet>
    <EntitySet Name="Cities1" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="Cities2" EntityType="System.Web.OData.Builder.City" />
  </EntityContainer>
</Schema>
```
