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

            var task = TaskFactory.Create(command.Title, command.Description, command.Branch, command.TypeId);

            await repository.SaveAsync(task, ct);

            return task.Id;
        }

        public async System.Threading.Tasks.Task Complete(CompleteTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var task = await repository.GetTaskByTitle(command.TaskTitle, ct)
                ?? throw new Exception($"Task with name {command.TaskTitle} not found");

            task.Complete();

            await repository.SaveAsync(task, ct);
        }

        public async Task<List<TaskDto>> GetLatestFinisheds(CancellationToken ct = default)
        {
            var tasks = await repository.GetLatestFinished(ct);

            return tasks!.ToDto();
        }

        public async System.Threading.Tasks.Task Update(UpdateTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var task = await repository.GetTask(command.Id, ct)
                ?? throw new Exception($"Task with id {command.Id} not found");

            task.Update(command.Title, command.Description, command.Branch);

            await repository.SaveAsync(task, ct);
        }
    }
}
