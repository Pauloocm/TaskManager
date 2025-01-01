namespace TaskManager.Domain.Tasks
{
    public class Task
    {
        public Task()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Status = TaskStatus.InProgress;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public TaskStatus Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public void Complete()
        {
            if (Status == TaskStatus.Done) return;

            Status = TaskStatus.Done;
            CompletedAt = DateTime.UtcNow;
        }

        public void Update(string title, string description, string branch)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(branch);

            if (Status == TaskStatus.Done)
                throw new Exception("Cannot update a completed task");

            Title = title;
            Description = description;
            Branch = branch;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
