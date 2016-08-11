
### Add binding path in NavigationSourceConfiguration{TEntityType}

We will provide the following APIs in navigation source configuration class to add binding path:

```c#
1. HasManyPath()
2. HasRequiredPath()
3. HasOptionalPath()
```

Each of these functions will return an object of the following generic type:

```xml
BindingPathConfiguration<TStructuralType>
```

### Add binding path in BindingPathConfiguration{TStructuralType}

The class also provide the following APIs to add binding path:

```c#
1. HasManyPath()
2. HasRequiredPath()
3. HasOptionalPath()
```

So, the developer can call as:

```C#
.HasManyPath(..).HasRequiredPath().HasManyPath()...
```

### Add target binding in BindingPathConfiguration{TStructuralType}

This class also provide the following APIs to add target binding and end the binding path:

```c#
1. HasManyBinding()
2. HasRequiredBinding()
3. HasOptionalBinding()
```

So, the normal navigation property binding configuration flow is：

1. Call `Has*Path()` from `NavigationSourceConfiguration{TEntityType}`
2. Continue to Call `Has*Path()` from the above returned object 
3. Repeat step-2 if necessary
4. Call `Has*Binding()` to add the target navigation source.

Here's an example:

```C#
customers.HasRequiredPath(c => c.Pet)
         .HasRequiredPath((Human p) => p.HumanAddress)
         .HasRequiredBinding(c => c.SubCity, "SubCities");
```
