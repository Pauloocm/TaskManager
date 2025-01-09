using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Infrastructure.Database;
using Task = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Infrastructure.Repositorie
{
    public class TaskRepository(DataContext dataContext) : ITaskRepository
    {
        private readonly DataContext dataContext = dataContext
            ?? throw new ArgumentNullException(nameof(dataContext));

        public async System.Threading.Tasks.Task Add(Task task, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(task);

            await dataContext.Task.AddAsync(task, ct);
        }

        public Task<Task?> GetTask(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));

            return dataContext.Task.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public Task<Task?> GetTask(string taskTitle, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(taskTitle);

            return dataContext.Task.FirstOrDefaultAsync(t => t.Title == taskTitle, ct);
        }

        public async System.Threading.Tasks.Task Commit(CancellationToken ct = default)
        {
            await dataContext.SaveChangesAsync(ct);
        }


        public async Task<IEnumerable<Task?>> SearchTasks(string? term, int page, bool Done = false, CancellationToken ct = default)
        {
            IQueryable<Task?> tasksQuery = dataContext.Task;

            if (Done)
            {
                tasksQuery = tasksQuery.Where(t => t!.Status == TaskStatus.Done);
            }
            else
            {
                tasksQuery = tasksQuery.Where(t => t!.Status == TaskStatus.InProgress);
            }

            if (!string.IsNullOrWhiteSpace(term))
            {
                tasksQuery = tasksQuery.Where(t => t!.Title.Contains(term) || t.Description.Contains(term) || t.Branch.Contains(term));
            }

            var test = tasksQuery.Where(t => t!.Status == TaskStatus.Done).ToList();
            var tasks = await tasksQuery.Skip((page - 1) * 5).Take(5).ToListAsync(ct);

            return [.. tasks];
        }
    }
}
