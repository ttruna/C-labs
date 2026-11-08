namespace TasksApp.Services;

public class SimpleTimeService : ITimeService
{
    public DateTime GetTimeForTomorrow() => DateTime.Now.AddDays(1);
}
