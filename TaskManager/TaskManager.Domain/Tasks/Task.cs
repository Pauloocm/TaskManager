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
            if (Status == TaskStatus.Done)
            {
                throw new InvalidOperationException("Task is already completed");
            }

            Status = TaskStatus.Done;
            CompletedAt = DateTime.UtcNow;
        }
    }
}
