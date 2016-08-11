
## Multiple path binding with inheritance

# CLR models

## Entity types
```C#
public class Cusomter
{
  public int Id { get; set; }

  public Animal Pet { get; set; }
}

public class VipCustomer : Cusomter
{
  public List<Address> VipLocations { get; set; }
}

public class City
{
  public int Id { get; set; }
}
```

## Complex types
```C#
public class Animal
{
}

public class Human : Animal
{
    public UsAddress HumanAddress { get; set; }
}

public class Horse : Animal
{
    public UsAddress HorseAddress { get; set; }
    public IList<UsAddress> HorseAddresses { get; set; }
}
public class Address
{
  public City City { get; set; }
}

public class UsAddress : Address
{
  public City SubCity { get; set; }
}
```

# Add navigation property binding:

```C#

customers.HasManyPath((VipCustomer v) => v.VipLocations).HasRequiredBinding(a => a.City, "A");
customers.HasManyPath((VipCustomer v) => v.VipLocations).HasRequiredBinding((UsAddress a) => a.SubCity, "B");

var pet = customers.HasRequiredPath(c => c.Pet);


pet.HasRequiredPath((Human p) => p.HumanAddress)
    .HasRequiredBinding(c => c.SubCity, "HumanCities");

pet.HasRequiredPath((Horse h) => h.HorseAddress).HasRequiredBinding(c => c.SubCity, "HorseCities");

pet.HasManyPath((Horse h) => h.HorseAddresses).HasRequiredBinding(c => c.SubCity, "HorseCities");
  
```

So, we can get the following target binding:

```xml

<Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
  <EntityContainer Name="Container">
    <EntitySet Name="Customers" EntityType="System.Web.OData.Builder.Cusomter">
      <NavigationPropertyBinding Path="Pet/System.Web.OData.Builder.Human/HumanAddress/SubCity" Target="HumanCities" />
      <NavigationPropertyBinding Path="Pet/System.Web.OData.Builder.Horse/HorseAddress/SubCity" Target="HorseCities" />
      <NavigationPropertyBinding Path="Pet/System.Web.OData.Builder.Horse/HorseAddresses/SubCity" Target="HorseCities" />
      <NavigationPropertyBinding Path="System.Web.OData.Builder.VipCustomer/VipLocations/System.Web.OData.Builder.UsAddress/SubCity" Target="B" />
      <NavigationPropertyBinding Path="System.Web.OData.Builder.VipCustomer/VipLocations/City" Target="A" />
    </EntitySet>
    <EntitySet Name="A" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="B" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="HumanCities" EntityType="System.Web.OData.Builder.City" />
    <EntitySet Name="HorseCities" EntityType="System.Web.OData.Builder.City" />
  </EntityContainer>
</Schema>
```
