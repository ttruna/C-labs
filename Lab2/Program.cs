using TasksApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    // form fields may be empty; don't treat non-nullable strings as [Required]
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// In modern ASP.NET Core the Startup.ConfigureServices() code lives here.
builder.Services.AddTransient<ITimeService, SimpleTimeService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
