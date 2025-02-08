namespace TaskManager.Domain.Tasks.Exceptions
{
    public class TaskAlreadyExistsException(string title) : Exception($"Task with the specified title {title} already exists")
    {
    }
}
