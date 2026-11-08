namespace TasksApp.Data;

/// <summary>Hard-coded demo data used by the controller.</summary>
public static class DemoData
{
    public static readonly List<string> Supermarkets = new()
    {
        "Silpo", "ATB", "Novus", "Fora", "Varus"
    };

    // product name -> quantity
    public static readonly Dictionary<string, int> ShoppingList = new()
    {
        ["Milk"] = 2,
        ["Bread"] = 1,
        ["Eggs"] = 10,
        ["Apples"] = 5,
        ["Cheese"] = 1
    };
}
