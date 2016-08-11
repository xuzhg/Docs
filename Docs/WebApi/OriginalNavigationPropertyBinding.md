## We have the following fluent APIs to add navigation property and navigation property binding:

### CLR Model

```C#
public class Customer
{
  public int Id { get; set; }
  
  public Order SingleOrder {get;set;}  // single navigation property
  
  public IList<Order> Orders {get;set;} // collection navigation property
}

public class Order
{
  public int Id { get; set; }
}
```

### Add navigation property

The following APIs are used to add navigation property. (from Schema)

1. HasMany()
2. HasRequired()
3. HasOptional()

So, we can do as:

```C#
ODataModelBuilder builder = new ODataModelBuilder();
var order = builder.EntityType<Order>().HasKey(o => o.Id);
var customer = builder.EntityType<Cusomter>().HasKey(c => c.Id);
customer.HasMany(c => c.Orders);
customer.HasRequired(c => c.SingleOrder);
```

We can get the following result:

```xml
<EntityType Name="Cusomter">
  <Key>
    <PropertyRef Name="Id" />
  </Key>
  <Property Name="Id" Type="Edm.Int32" Nullable="false" />
  <NavigationProperty Name="Orders" Type="Collection(System.Web.OData.Builder.Order)" />
  <NavigationProperty Name="SingleOrder" Type="System.Web.OData.Builder.Order" Nullable="false" />
</EntityType>
```

### Add navigation property binding

The following APIs are used to add navigation property binding. (from entity set container)

1. HasManyBinding()
2. HasRequiredBinding()
3. HasOptionalBinding()

So, we can do as:

```C#
ODataModelBuilder builder = new ODataModelBuilder();
var customers = builder.EntitySet<Cusomter>("Customers");
customers.HasManyBinding(c => c.Orders, "Orders");
customers.HasRequiredBinding(c => c.SingleOrder, "SingleOrders");
```
We can get the following result:

```xml
<EntityContainer Name="Container">
  <EntitySet Name="Customers" EntityType="System.Web.OData.Builder.Cusomter">
    <NavigationPropertyBinding Path="Orders" Target="Orders" />
    <NavigationPropertyBinding Path="SingleOrder" Target="SingleOrders" />
  </EntitySet>
  <EntitySet Name="Orders" EntityType="System.Web.OData.Builder.Order" />
  <EntitySet Name="SingleOrders" EntityType="System.Web.OData.Builder.Order" />
</EntityContainer>
```

### Navigation property binding path

The binding path is straight-forward, it's the navigation property name.

It doesn't support to:

1. Add the complex property into path
2. Add the type cast into path

