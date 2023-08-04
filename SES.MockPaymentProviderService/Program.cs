var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/payment/{id}", (string id) =>
{
    Thread.Sleep(5000);
    return id;
});

app.Run();