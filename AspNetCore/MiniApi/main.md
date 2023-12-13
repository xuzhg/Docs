
```C#
public delegate Task RequestDelegate(HttpContext context);
```



```C#
app.MapGet("/", () => "Hello world");
```

Basically, the above lamda should be converted into a function as:

```C#
Task Invoke(HttpContext httpContext)
{
    // handler is the original lambda method, it returns "Hello World" and no parameter
    string result = handler.Invoke();

    // The return value is written to the response
    // Since it's string, the context type is 'text/plain'
    httpContext.Response.ContentType ??= "text/plain; charset=utf-8";
    return httpContext.Response.WriteAsync(result);
}
```

Be noted: the Minimal API uses `Expression` to build the above method.
