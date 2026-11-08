using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TriangleApp.Models;

namespace TriangleApp.Filters;

/// <summary>Turns InvalidTriangleException into an error page (HTTP 400).</summary>
public class InvalidTriangleFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _metadata;
    public InvalidTriangleFilter(IModelMetadataProvider metadata) => _metadata = metadata;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not InvalidTriangleException ex) return;

        context.Result = new ViewResult
        {
            ViewName = "InvalidTriangle",
            StatusCode = StatusCodes.Status400BadRequest,
            ViewData = new ViewDataDictionary<string>(_metadata, context.ModelState)
            {
                Model = ex.Message
            }
        };
        context.ExceptionHandled = true;
    }
}
