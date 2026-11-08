namespace TasksApp.Services;

public interface ITimeService
{
    /// <summary>Time for shopping "just in a day" from now.</summary>
    DateTime GetTimeForTomorrow();
}
