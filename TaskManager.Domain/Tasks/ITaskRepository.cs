
namespace TaskManager.Domain.Tasks
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task Add(Task task, CancellationToken ct = default);
        Task<IEnumerable<Task?>> SearchTasks(string? term, int page, bool Done = false, CancellationToken ct = default);
        Task<Task?> GetTask(Guid id, CancellationToken ct);

        Task<Task?> GetTask(string taskTitle, CancellationToken ct);

        System.Threading.Tasks.Task Commit(CancellationToken ct = default);
    }
}
