namespace TriangleApp.Models;

public class InvalidTriangleException : Exception
{
    public InvalidTriangleException(string message) : base(message) { }
}
