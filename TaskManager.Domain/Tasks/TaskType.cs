namespace TaskManager.Domain.Tasks
{
    public class TaskType
    {
        private static readonly List<TaskType> AllTypes = [];

        public static readonly TaskType Feature = new(1, "Feature");
        public static readonly TaskType Bug = new(2, "Bug");

        public string Type { get; set; } = null!;
        public int Id { get; set; }


        public TaskType(int id, string status)
        {
            Id = id;
            Type = status;
            AllTypes.Add(this);
        }

        public static TaskType? GetById(int id)
        {
            return AllTypes.FirstOrDefault(x => x.Id == id);
        }
    }
}
