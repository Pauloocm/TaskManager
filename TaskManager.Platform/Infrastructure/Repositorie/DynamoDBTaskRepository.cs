using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Infrastructure.Models;
using Task = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Platform.Infrastructure.Repositorie
{
    public class DynamoDBTaskRepository(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB dynamoClient) : ITaskRepository
    {
        private readonly IDynamoDBContext context = dynamoDbContext
            ?? throw new ArgumentNullException(nameof(dynamoDbContext));

        private readonly IAmazonDynamoDB dynamoDbClient = dynamoClient
            ?? throw new ArgumentNullException(nameof(dynamoClient));

        private readonly string TableName = "Tasks";


        private const string KeyConditionStatusId = "StatusId = :statusId";
        private const string StatusIdSKIndex = "StatusId-SK-index";
        private const string KeyConditionStatusIdAndBeginsWithSk = "StatusId = :statusId AND begins_with(SK, :sk)";
        private const string KeyConditionBeginsWithPKAndBeginsWithSk = "PK = :pk AND begins_with(SK, :sk)";

        public async System.Threading.Tasks.Task SaveAsync(Task task, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(task);

            await context.SaveAsync(task.ToModel(task.Id, task.CreatedAt), ct);
        }

        public async Task<Task?> GetTask(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));

            var queryConfig = new QueryOperationConfig
            {
                KeyExpression = new Expression
                {
                    ExpressionStatement = KeyConditionBeginsWithPKAndBeginsWithSk,
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    { ":pk", $"TASK#{id}" },
                    { ":sk", "CREATEDAT#" }
                }
                }
            };

            var search = context.FromQueryAsync<TaskModel>(queryConfig);
            var results = await search.GetRemainingAsync(ct);

            return results.SingleOrDefault()?.ToDomain();
        }

        public async Task<Task?> GetTaskByTitle(string taskTitle, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(taskTitle);

            var queryConfig = new QueryRequest
            {
                TableName = TableName,
                FilterExpression = "Title = :title",
                KeyConditionExpression = KeyConditionStatusIdAndBeginsWithSk,
                IndexName = StatusIdSKIndex,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                 {
                    { ":statusId", new AttributeValue { N = "1" } },
                    { ":sk", new AttributeValue { S = "CREATEDAT#" } },
                    { ":title", new AttributeValue { S = $"{taskTitle}" } }
                },
            };

            var response = await dynamoDbClient.QueryAsync(queryConfig, ct);

            return response.Items.SingleOrDefault().ToDomain();
        }

        public async Task<IEnumerable<Task?>> GetLatestFinished(CancellationToken ct = default)
        {
            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = StatusIdSKIndex,
                KeyConditionExpression = KeyConditionStatusId,

                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {

                    { ":statusId", new AttributeValue { N = "1" } }
                },

                ScanIndexForward = true,
                Limit = 5
            };

            var response = await dynamoDbClient.QueryAsync(queryRequest, ct);

            return response.ToDomain();
        }
    }
}
