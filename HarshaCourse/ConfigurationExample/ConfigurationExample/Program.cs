using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection("weatherapi")); 
var app = builder.Build();
// load MyOwnConfig.json
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
