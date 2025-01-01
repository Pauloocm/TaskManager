namespace TaskManager.Platform.Application
{
    public record UpdateTaskCommand(Guid Id, string Title, string Description, string Branch);
}
