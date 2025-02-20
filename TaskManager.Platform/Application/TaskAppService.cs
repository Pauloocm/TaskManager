using Microsoft.Extensions.Logging;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Tasks.Exceptions;
using TaskManager.Platform.Application.Commands;
using TaskFactory = TaskManager.Domain.Tasks.TaskFactory;

namespace TaskManager.Platform.Application
{
    public class TaskAppService(ITaskRepository taskRepository, ILogger<TaskAppService> logger) : ITaskAppService
    {
        private readonly ITaskRepository repository = taskRepository ??
            throw new ArgumentNullException(nameof(taskRepository));

        private readonly ILogger<TaskAppService> logger = logger ??
            throw new ArgumentNullException(nameof(logger));

        public async Task<Guid> Add(AddTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            logger.LogInformation("Starting AddTask process for task {Title}", command.Title);

            logger.LogDebug("Validating task with title '{Title}'", command.Title);

            var existentTask = await repository.GetTaskByTitle(command.Title, ct);

            if (existentTask is not null) throw new TaskAlreadyExistsException(command.Title);

            logger.LogDebug("Creating new task with properties: {@TaskProperties}", new
            {
                command.Title,
                command.Description,
                command.Branch,
                command.TypeId
            });

            var task = TaskFactory.Create(command.Title, command.Description, command.Branch, command.TypeId);

            logger.LogInformation("Task created successfully. TaskId: {Id}", task.Id);

            await repository.SaveAsync(task, ct);

            logger.LogInformation("Task persisted successfully. TaskId: {Id}", task.Id);

            return task.Id;
        }

        public async System.Threading.Tasks.Task Complete(CompleteTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            logger.LogInformation("Starting CompleteTask process for task {Title}", command.Id);

            var task = await repository.GetTask(command.Id, ct) ?? throw new TaskNotFoundException();

            logger.LogInformation("Task returned successfully. TaskId: {Id}", task.Id);

            task.Complete();

            logger.LogInformation("Task completed successfully. TaskId: {Id}", task.Id);

            await repository.SaveAsync(task, ct);

            logger.LogInformation("Task persisted successfully. TaskId: {Id}", task.Id);
        }

        public async Task<List<TaskDto>> GetInProgressTasks(CancellationToken ct = default)
        {
            logger.LogInformation("Starting GetInProgress process");

            var tasks = await repository.GetInProgress(ct);

            logger.LogInformation("Retrieved {Count} InProgress tasks", tasks.Count());

            return tasks!.ToDto();
        }

        public async Task<List<TaskDto>> GetLatestFinisheds(CancellationToken ct = default)
        {
            logger.LogInformation("Starting GetLatestFinisheds process");

            var tasks = await repository.GetLatestFinished(ct);

            logger.LogInformation("Retrieved {Count} Finisheds tasks", tasks.Count());

            return tasks!.ToDto();
        }

        public async System.Threading.Tasks.Task Update(UpdateTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            logger.LogInformation("Starting Update process for task {Id}", command.Id);

            var task = await repository.GetTask(command.Id, ct) ?? throw new TaskNotFoundException();

            logger.LogInformation("Task returned successfully. TaskId: {Id}", task.Id);

            logger.LogDebug("Updating task with new properties: {@TaskProperties}", new
            {
                command.Title,
                command.Description,
                command.Branch
            });

            task.Update(command.Title, command.Description, command.Branch);

            logger.LogInformation("Task updated successfully. TaskId: {Id}", task.Id);

            await repository.SaveAsync(task, ct);

            logger.LogInformation("Task persisted successfully. TaskId: {Id}", task.Id);
        }
    }
}
