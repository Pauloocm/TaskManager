using Amazon.DynamoDBv2.Model;
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
                CreatedAt = task.CreatedAt.ToUniversalTime(),
            };

            if (task.CompletedAt.HasValue || task.UpdatedAt.HasValue)
            {
                model.CompletedAt = task?.CompletedAt?.ToUniversalTime();
                model.UpdatedAt = task?.UpdatedAt?.ToUniversalTime();
            }

            model.SetKeys(task!.Id, task.CreatedAt);

            return model;
        }

        public static Task? ToDomain(this Dictionary<string, AttributeValue>? queryResponse)
        {
            if (queryResponse is null) return null;

            var task = new Task()
            {
                Id = Guid.Parse(queryResponse["Id"].S),
                Title = queryResponse["Title"].S is null ? "" : queryResponse["Title"].S,
                Description = queryResponse["Description"].S is null ? "" : queryResponse["Description"].S,
                Branch = queryResponse["Branch"].S is null ? "" : queryResponse["Branch"].S,
                Status = TaskStatus.GetById(int.Parse(queryResponse["StatusId"].N))!,
                Type = TaskType.GetById(int.Parse(queryResponse["TypeId"].N))!,
                CreatedAt = DateTime.Parse(queryResponse["CreatedAt"].S).ToLocalTime(),
                UpdatedAt = queryResponse.TryGetValue("UpdatedAt", out AttributeValue? value) ? DateTime.Parse(value.S).ToLocalTime() : null,
                CompletedAt = queryResponse.TryGetValue("CompletedAt", out AttributeValue? date) ? DateTime.Parse(date.S).ToLocalTime() : null
            };

            return task;
        }

        public static List<Task?> ToDomain(this QueryResponse? queryResponse)
        {
            if (queryResponse is null) return [];

            return queryResponse.Items.Where(qr => qr is not null).Select(qr => qr.ToDomain()).ToList();
        }
    }
}
