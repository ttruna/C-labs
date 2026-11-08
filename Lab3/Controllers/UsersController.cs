using Microsoft.AspNetCore.Mvc;
using ProductsApp.Models;

namespace ProductsApp.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private const string Secret = "df2323eoT";

    private static readonly List<AppUser> Users = new()
    {
        new AppUser { Id = 1, Name = "Olena Kovalenko", Email = "olena@example.com",  Role = "Admin" },
        new AppUser { Id = 2, Name = "Andrii Shevchenko", Email = "andrii@example.com", Role = "Manager" },
        new AppUser { Id = 3, Name = "Maria Bondar",     Email = "maria@example.com",  Role = "Customer" },
        new AppUser { Id = 4, Name = "Petro Melnyk",     Email = "petro@example.com",  Role = "Customer" }
    };

    /// <summary>
    /// The secret is accepted ONLY from the request body (form data), never from the route or query string.
    /// It is accepted as a form field whose value is the secret (e.g. key=df2323eoT)
    /// or a field whose name is the secret.
    /// </summary>
    private bool IsAuthorized()
    {
        if (!Request.HasFormContentType) return false;
        var form = Request.Form;
        return form.ContainsKey(Secret) || form.Any(kv => kv.Value.Any(v => v == Secret));
    }

    // a small page with a form that POSTs the key in the request body
    [HttpGet("login")]
    public IActionResult Login() => View();

    // a plain GET has no body -> unauthorized
    [HttpGet("index")]
    public IActionResult IndexGet() => Unauthorized();

    // /users/index  (POST, key in the body)
    [HttpPost("index")]
    public IActionResult Index()
    {
        if (!IsAuthorized()) return Unauthorized();
        return View(Users);
    }
}
