using System.Globalization;
using Microsoft.AspNetCore.Localization;
using ProductsApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// one shared in-memory store for the whole app
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

// fixed culture so decimals always use a dot ("42.50"), whatever the OS language is
var enUS = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
});

app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/products"));
app.MapControllers();   // attribute routing

app.Run();
