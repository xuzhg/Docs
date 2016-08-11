
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

