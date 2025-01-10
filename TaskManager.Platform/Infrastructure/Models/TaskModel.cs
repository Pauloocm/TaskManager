using Amazon.DynamoDBv2.DataModel;
using TaskManager.Domain.Tasks;

namespace TaskManager.Platform.Infrastructure.Models
{
    [DynamoDBTable("Tasks")]
    public class TaskModel
    {

        private string pk = null!;
        private string sk = null!;

        [DynamoDBHashKey("PK")]
        public string PK
        {
            get => pk;
            set
            {
                pk = value;
                if (!string.IsNullOrEmpty(pk))
                {
                    var idString = pk.Replace("TASK#", "");
                    Id = Guid.Parse(idString);
                }
            }
        }

        [DynamoDBRangeKey("SK")]
        public string SK
        {
            get => sk;
            set
            {
                sk = value;
                if (!string.IsNullOrEmpty(sk))
                {
                    Title = sk.Replace("TITLE#", "");
                }
            }
        }

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public void SetKeys(Guid id, string title)
        {
            Id = id;
            Title = title;
            PK = $"TASK#{id}";
            SK = $"TITLE#{title}";
        }
    }
}
