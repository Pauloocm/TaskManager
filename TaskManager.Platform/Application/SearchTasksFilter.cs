namespace TaskManager.Platform.Application
{
    public record SearchTasksFilter(int Page, string? Term = null, bool Done = false);
}
