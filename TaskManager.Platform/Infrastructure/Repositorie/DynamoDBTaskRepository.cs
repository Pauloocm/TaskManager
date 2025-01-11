using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Infrastructure.Database;
using TaskManager.Platform.Infrastructure.Models;
using Task = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Infrastructure.Repositorie
{
    public class DynamoDBTaskRepository(IDynamoDBContext dynamoDbContext) : ITaskRepository
    {
        private readonly IDynamoDBContext context = dynamoDbContext
            ?? throw new ArgumentNullException(nameof(dynamoDbContext));

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
                    ExpressionStatement = "PK = :pk",
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    { ":pk", $"TASK#{id}" }
                }
                }
            };

            var search = context.FromQueryAsync<TaskModel>(queryConfig);
            var results = await search.GetRemainingAsync(ct);

            return results.SingleOrDefault()?.ToDomain();
        }

        public Task<Task?> GetTask(string taskTitle, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(taskTitle);

            //return dataContext.Task.FirstOrDefaultAsync(t => t.Title == taskTitle, ct);

            return null;
        }

        //public async System.Threading.Tasks.Task Commit(CancellationToken ct = default)
        //{

        //}

        //public async System.Threading.Tasks.Task Add(Task task, CancellationToken ct = default)
        //{
        //    ArgumentNullException.ThrowIfNull(task);

        //    await dataContext.Task.AddAsync(task, ct);
        //}






        //public async Task<IEnumerable<Task?>> SearchTasks(string? term, int page, bool Done = false, CancellationToken ct = default)
        //{
        //    IQueryable<Task?> tasksQuery = dataContext.Task;

        //    if (Done)
        //    {
        //        tasksQuery = tasksQuery.Where(t => t!.Status == TaskStatus.Done);
        //    }
        //    else
        //    {
        //        tasksQuery = tasksQuery.Where(t => t!.Status == TaskStatus.InProgress);
        //    }

        //    if (!string.IsNullOrWhiteSpace(term))
        //    {
        //        tasksQuery = tasksQuery.Where(t => t!.Title.Contains(term) || t.Description.Contains(term) || t.Branch.Contains(term));
        //    }

        //    var test = tasksQuery.Where(t => t!.Status == TaskStatus.Done).ToList();
        //    var tasks = await tasksQuery.Skip((page - 1) * 5).Take(5).ToListAsync(ct);

        //    return [.. tasks];
        //}
    }
}
