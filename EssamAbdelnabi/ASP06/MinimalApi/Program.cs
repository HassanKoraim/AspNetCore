using MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IBookServices, BookServices>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/books", (IBookServices services) => services.getAll());

app.Run();
