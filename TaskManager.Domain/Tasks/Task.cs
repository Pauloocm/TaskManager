namespace TaskManager.Domain.Tasks
{
    public class Task
    {
        public Task()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Status = TaskStatus.InProgress;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public TaskType Type { get; set; } = null!;
        public TaskStatus Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public void Complete()
        {
            if (Status == TaskStatus.Done) return;

            Status = TaskStatus.Done;
            CompletedAt = DateTime.Now;
        }


        public void Update(string title, string description, string branch)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            ArgumentException.ThrowIfNullOrWhiteSpace(branch);

            if (Status == TaskStatus.Done)
                throw new Exception("Cannot update a completed task");

            Title = title;
            Description = description;
            Branch = branch;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
