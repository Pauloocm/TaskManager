using Task = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Platform.Application
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public string Status { get; set; } = null!;
    }

    public static class Extension
    {
        public static TaskDto? ToDto(this Task task)
        {
            if (task is null) return null;

            var dto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Branch = task.Branch,
                CreatedAt = task.CreatedAt,
                FinishedAt = task.CompletedAt,
                Status = task.Status.Status
            };

            return dto;
        }

        public static List<TaskDto> ToDto(this IEnumerable<Task> tasks)
        {
            if (tasks is null || tasks.Any() == false) return Enumerable.Empty<TaskDto>().ToList();

            return tasks.Where(t => t is not null).Select(t => t.ToDto()).ToList()!;
        }
    }
}
