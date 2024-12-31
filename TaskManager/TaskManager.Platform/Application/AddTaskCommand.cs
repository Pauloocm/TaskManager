namespace TaskManager.Platform.Application
{
    public record AddTaskCommand(string Title, string Description, string Branch);
}
