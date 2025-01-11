using TaskManager.Domain.Tasks;
using Task = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Infrastructure.Models
{
    public static class ModelExtensions
    {
        public static Task ToDomain(this TaskModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var status = TaskStatus.GetById(model.StatusId);
            var type = TaskType.GetById(model.TypeId);

            if (status is null || type is null)
            {
                throw new Exception("Invalid status or type");
            }

            var task = new Task()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Branch = model.Branch,
                Status = status,
                Type = type,
                CompletedAt = model.CompletedAt,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return task;
        }

        public static TaskModel ToModel(this Task task)
        {
            ArgumentNullException.ThrowIfNull(task);

            var model = new TaskModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Branch = task.Branch,
                StatusId = task.Status.Id,
                TypeId = task.Type.Id,
                CompletedAt = task.CompletedAt,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            model.SetKeys(task.Id, task.CreatedAt);

            return model;
        }

        public static TaskModel ToModel(this Task task, Guid id, DateTime createdAt)
        {
            ArgumentNullException.ThrowIfNull(task);

            var model = new TaskModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Branch = task.Branch,
                StatusId = task.Status.Id,
                TypeId = task.Type.Id,
                CompletedAt = task.CompletedAt,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            model.SetKeys(task.Id, task.CreatedAt);

            return model;
        }
    }
}
