using TaskManager.Domain.Tasks.Exceptions;

namespace TaskManager.Domain.Tasks
{
    public static class TaskFactory
    {
        public static Task Create(string title, string description, string branch, int typeId)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(branch);

            if (typeId <= 0)
                throw new ArgumentException("Type Id must be greater than 0");

            TaskType type = TaskType.GetById(typeId)
                ?? throw new InvalidTaskTypeException(typeId);

            var task = new Task
            {
                Title = title,
                Description = description,
                Branch = branch,
                Type = type
            };

            ArgumentNullException.ThrowIfNull(task);

            return task;

        }
    }
}
