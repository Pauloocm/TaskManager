namespace TaskManager.Domain.Tasks.Exceptions
{
    public class InvalidStatusException(int statusId) : Exception($"Invalid Task status id: {statusId}")
    {
    }
}
