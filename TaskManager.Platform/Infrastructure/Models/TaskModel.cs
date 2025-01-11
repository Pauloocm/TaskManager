using Amazon.DynamoDBv2.DataModel;

namespace TaskManager.Platform.Infrastructure.Models
{
    [DynamoDBTable("Tasks")]
    public class TaskModel
    {
        [DynamoDBHashKey("PK")]
        public string PK { get; set; } = null!;

        [DynamoDBRangeKey("SK")]
        public string SK { get; set; } = null!;

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public void SetKeys(Guid id, DateTime createdAt)
        {
            PK = $"TASK#{id}";
            SK = $"CREATEDAT#{createdAt.ToUniversalTime():yyyy-MM-dd'T'HH:mm:ss'Z'}";
        }
    }
}
