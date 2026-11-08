using Microsoft.AspNetCore.Mvc;

namespace ProductsApp.Controllers;

[Route("error")]
public class ErrorController : Controller
{
    // custom 404 page for a missing product
    [HttpGet("productnotfound")]
    public IActionResult ProductNotFound()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View();
    }
}
