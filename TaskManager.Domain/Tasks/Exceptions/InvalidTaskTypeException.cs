namespace TaskManager.Domain.Tasks.Exceptions
{
    public class InvalidTaskTypeException(int typeId) : Exception($"Invalid Task type id: {typeId}")
    {
    }
}
