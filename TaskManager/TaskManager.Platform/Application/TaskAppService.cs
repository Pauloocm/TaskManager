using TaskManager.Domain.Tasks;
using TaskFactory = TaskManager.Domain.Tasks.TaskFactory;

namespace TaskManager.Platform.Application
{
    public class TaskAppService(ITaskRepository taskRepository) : ITaskAppService
    {
        private readonly ITaskRepository repository = taskRepository ??
            throw new ArgumentNullException(nameof(taskRepository));

        public async Task<Guid> Add(AddTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var task = TaskFactory.Create(command.Title, command.Description, command.Branch);

            await repository.Add(task, ct);
            await repository.Commit(ct);

            return task.Id;
        }

        public async System.Threading.Tasks.Task Complete(CompleteTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var task = await repository.GetTask(command.Id, ct)
                ?? throw new Exception($"Task with id {command.Id} not found");

            task.Complete();

            await repository.Commit(ct);
        }

        public async Task<List<TaskDto>> Search(SearchTasksFilter filter, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            var tasks = await repository.SearchTasks(filter.Term, filter.Page, ct);

            return tasks!.ToDto();
        }
    }
}
