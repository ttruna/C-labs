var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapGet("/", () => Results.Text(
    "Triangle app is running. Try: /triangle/Perimeter?side1=10&side2=20&side3=25"));
app.MapControllers();   // attribute routing: /triangle/...

app.Run();
