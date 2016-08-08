
# CLR models

## Entity types
```C#
public class Cusomter
{
  public int Id { get; set; }
  public Address Location { get; set; }
  public Address HomeLocation { get; set; }
  
  [Contained]
  public Order OrderLine { get; set; }
}

public class VipCusomter : Customer
{
  public Address VipLocation { get; set; }
}

public class City
{
  public int Id { get; set; }
}

public class Order
{
  public int Id { get; set; }
  
  public Address DeliverAddress { get; set; }
}
```

## Complex types
```C#
public class Address
{
  public City City1 { get; set; }
  public City City2 
  
  public string Street { get; set; }
}

public class UsAddress : Address
{
  public City SubCity { get; set; }
  
}
```

## Entity Set

```xml
<EntitySet Name="Customers" EntityType="NS.Cusomter">
<EntitySet Name="CitiesA" EntityType="NS.City">
<EntitySet Name="CitiesB" EntityType="NS.City">
```

## Naivgation property binding

```xml
<EntitySet Name="Customers" EntityType="NS.Cusomter">
   <NavigationPropertyBinding Path="Location/Cityes" Target="CitiesA" />
   <NavigationPropertyBinding Path="HomeLocation/Cityes" Target="CitiesB" />
   <NavigationPropertyBinding Path="NS.VipCustomer/VipLocation/Cityes" Target="CitiesA" />
   
   <NavigationPropertyBinding Path="Location/NS.SubAddress/SubCity" Target="CitiesA" />
   
   <NavigationPropertyBinding Path="OrderLine/DeliverAddress/Cityes" Target="CitiesA" />
</EntitySet>
```
