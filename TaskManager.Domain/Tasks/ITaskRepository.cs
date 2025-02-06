namespace TaskManager.Domain.Tasks
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task SaveAsync(Task task, CancellationToken ct = default);
        Task<Task?> GetTask(Guid id, CancellationToken ct);
        Task<IEnumerable<Task?>> GetInProgress(CancellationToken ct = default);
        Task<IEnumerable<Task?>> GetLatestFinished(CancellationToken ct = default);
        Task<Task?> GetTaskByTitle(string taskTitle, CancellationToken ct);
    }
}
