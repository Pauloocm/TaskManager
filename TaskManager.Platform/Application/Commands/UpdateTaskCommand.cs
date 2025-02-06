namespace TaskManager.Platform.Application.Commands
{
    public class UpdateTaskCommand()
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;

        public UpdateTaskCommand SetId(Guid id)
        {
            Id = id;
            return this;
        }
    };
}
