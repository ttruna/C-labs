using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TriangleApp.Filters;
using TriangleApp.Models;

namespace TriangleApp.Controllers;

[Route("triangle")]
[TypeFilter(typeof(InvalidTriangleFilter))]
public class TriangleController : Controller
{
    private static ContentResult Text(string s) => new() { Content = s, ContentType = "text/plain; charset=utf-8" };
    private static ContentResult Bool(bool b) => Text(b ? "true" : "false");

    // ---- manual query parsing (no model binding) ----

    private double ReadSide(string key)
    {
        string? raw = Request.Query[key].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(raw))
            throw new InvalidTriangleException($"Missing query parameter '{key}'.");
        if (!double.TryParse(raw.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
            throw new InvalidTriangleException($"Parameter '{key}' = '{raw}' is not a number.");
        return v;
    }

    /// <param name="prefix">"" for side1.., "tr1." for tr1.side1.., "tr[0]." for tr[0].side1..</param>
    private Triangle ReadTriangle(string prefix = "")
        => new(ReadSide(prefix + "side1"), ReadSide(prefix + "side2"), ReadSide(prefix + "side3"));

    private List<Triangle> ReadTriangles(string name = "tr")
    {
        var list = new List<Triangle>();
        for (int i = 0; ; i++)
        {
            string prefix = $"{name}[{i}].";
            bool any = Request.Query.ContainsKey(prefix + "side1")
                    || Request.Query.ContainsKey(prefix + "side2")
                    || Request.Query.ContainsKey(prefix + "side3");
            if (!any) break;
            list.Add(ReadTriangle(prefix));
        }
        if (list.Count == 0)
            throw new InvalidTriangleException("No triangles were provided.");
        return list;
    }

    // ---- actions ----

    [HttpGet("Info")]
    public IActionResult Info() => Text(ReadTriangle().ToInfoString());

    [HttpGet("Area")]
    public IActionResult Area() => Text(Triangle.FormatNumber(ReadTriangle().Area));

    [HttpGet("Perimeter")]
    public IActionResult Perimeter() => Text(Triangle.FormatNumber(ReadTriangle().Perimeter));

    [HttpGet("IsRightAngled")]
    public IActionResult IsRightAngled() => Bool(ReadTriangle().IsRightAngled);

    [HttpGet("IsEquilateral")]
    public IActionResult IsEquilateral() => Bool(ReadTriangle().IsEquilateral);

    [HttpGet("IsIsosceles")]
    public IActionResult IsIsosceles() => Bool(ReadTriangle().IsIsosceles);

    [HttpGet("AreCongruent")]
    public IActionResult AreCongruent()
        => Bool(ReadTriangle("tr1.").IsCongruentTo(ReadTriangle("tr2.")));

    [HttpGet("AreSimilar")]
    public IActionResult AreSimilar()
        => Bool(ReadTriangle("tr1.").IsSimilarTo(ReadTriangle("tr2.")));

    // The task text spells it "GreatesByPerimeter"; both spellings are supported.
    [HttpGet("GreatesByPerimeter")]
    [HttpGet("GreatestByPerimeter")]
    public IActionResult GreatestByPerimeter()
        => Text(ReadTriangles().MaxBy(t => t.Perimeter)!.ToInfoString());

    [HttpGet("GreatestByArea")]
    public IActionResult GreatestByArea()
        => Text(ReadTriangles().MaxBy(t => t.Area)!.ToInfoString());

    [HttpGet("PairwiseNonSimilar")]
    public IActionResult PairwiseNonSimilar()
    {
        var chosen = new List<Triangle>();
        foreach (var t in ReadTriangles())
            if (!chosen.Any(c => c.IsSimilarTo(t)))
                chosen.Add(t);

        return Text(string.Join(Environment.NewLine, chosen.Select(t => t.ToInfoString())));
    }
}
