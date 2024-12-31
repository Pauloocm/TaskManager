namespace TaskManager.Domain.Tasks
{
    public class TaskFactory
    {
        public static Task Create(string title, string description, string branch)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(branch);

            var task = new Task
            {
                Title = title,
                Description = description,
                Branch = branch
            };

            ArgumentNullException.ThrowIfNull(task);

            return task;

        }
    }
}
