using System.Globalization;
using System.Text;

namespace TriangleApp.Models;

public sealed class Triangle
{
    // 0.001% of the first value
    private const double RelativeTolerance = 0.001 / 100.0;

    /// <summary>Sides in ascending order.</summary>
    public double[] Sides { get; }

    public Triangle(double a, double b, double c)
    {
        var s = new[] { a, b, c };
        if (s.Any(x => double.IsNaN(x) || double.IsInfinity(x) || x <= 0))
            throw new InvalidTriangleException("Sides must be positive finite numbers.");

        Array.Sort(s);
        if (s[0] + s[1] <= s[2])
            throw new InvalidTriangleException(
                $"Triangle ({a}, {b}, {c}) does not exist: the triangle inequality is violated.");

        Sides = s;
    }

    /// <summary>Equality of doubles: difference less than 0.001% of the first value.</summary>
    public static bool AreEqual(double first, double second)
        => Math.Abs(first - second) < Math.Abs(first) * RelativeTolerance;

    public double Perimeter => Sides.Sum();

    public double Area
    {
        get
        {
            double p = Perimeter / 2;
            return Math.Sqrt(p * (p - Sides[0]) * (p - Sides[1]) * (p - Sides[2]));
        }
    }

    public bool IsRightAngled
        => AreEqual(Sides[2] * Sides[2], Sides[0] * Sides[0] + Sides[1] * Sides[1]);

    public bool IsEquilateral
        => AreEqual(Sides[0], Sides[1]) && AreEqual(Sides[1], Sides[2]);

    public bool IsIsosceles
        => AreEqual(Sides[0], Sides[1]) || AreEqual(Sides[1], Sides[2]);

    public bool IsCongruentTo(Triangle other)
        => Enumerable.Range(0, 3).All(i => AreEqual(Sides[i], other.Sides[i]));

    public bool IsSimilarTo(Triangle other)
    {
        double k = other.Sides[0] / Sides[0];
        return Enumerable.Range(0, 3).All(i => AreEqual(Sides[i] * k, other.Sides[i]));
    }

    public static string FormatNumber(double v)
        => Math.Round(v, 4).ToString("0.####", CultureInfo.InvariantCulture);

    public string ToInfoString()
    {
        double p = Perimeter;
        var sb = new StringBuilder();
        sb.AppendLine(new string('-', 60));
        sb.AppendLine("Triangle:");
        sb.AppendLine($"({FormatNumber(Sides[0])}, {FormatNumber(Sides[1])}, {FormatNumber(Sides[2])})");
        sb.AppendLine("Reduced:");
        sb.AppendLine($"({FormatNumber(Sides[0] / p)}, {FormatNumber(Sides[1] / p)}, {FormatNumber(Sides[2] / p)})");
        sb.AppendLine();
        sb.AppendLine($"Area = {FormatNumber(Area)}");
        sb.AppendLine($"Perimeter = {FormatNumber(p)}");
        sb.AppendLine(new string('-', 60));
        return sb.ToString();
    }
}
