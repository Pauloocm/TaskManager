namespace TaskManager.Domain.Tasks
{
    public class TaskStatus
    {
        private static readonly List<TaskStatus> AllStatus = [];

        public static readonly TaskStatus InProgress = new(1, "In Progress");
        public static readonly TaskStatus Done = new(2, "Done");

        public string Status { get; set; } = null!;
        public int Id { get; set; }


        public TaskStatus(int id, string status)
        {
            Id = id;
            Status = status;
            AllStatus.Add(this);
        }

        public static TaskStatus? GetById(int id)
        {
            return AllStatus.FirstOrDefault(x => x.Id == id);
        }
    }
}
