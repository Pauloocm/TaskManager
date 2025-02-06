namespace TaskManager.Platform.Application.Commands
{
    public record AddTaskCommand(string Title, string Description, string Branch, int TypeId);
}
