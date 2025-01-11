using NSubstitute;
using TaskManager.Domain.Tasks;
using TaskManager.Platform.Application;
using Task = System.Threading.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Tests
{
    [TestFixture]
    public class TaskAppServiceTests
    {
        private ITaskRepository taskRepositoryMock = null!;
        private TaskAppService taskAppService = null!;

        [SetUp]
        public void Setup()
        {
            taskRepositoryMock = Substitute.For<ITaskRepository>();
            taskAppService = new TaskAppService(taskRepositoryMock);
        }

        [Test]
        public void Add_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Add(null!));
        }

        [Test]
        public async Task Add()
        {
            var command = new AddTaskCommand("title", "description", "branch", 1);

            var expectedTask = new Domain.Tasks.Task()
            {
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress,
                Type = TaskType.Feature
            };

            var id = await taskAppService.Add(command);

            await taskRepositoryMock.Received().SaveAsync(Arg.Is<Domain.Tasks.Task>(t =>
            t.Title == command.Title && t.Description == command.Description
            && t.Branch == command.Branch && t.Type.Id == command.TypeId));

            await taskRepositoryMock.Received().SaveAsync(Arg.Is<Domain.Tasks.Task>(t => t.Id == id));
        }

        [Test]
        public void Complete_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Complete(null!));
        }

        [Test]
        public async Task Complete()
        {
            var command = new CompleteTaskCommand("Feature authentication");

            var expectedTask = new Domain.Tasks.Task()
            {
                Id = Guid.NewGuid(),
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            taskRepositoryMock.GetTaskByTitle(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(expectedTask);

            await taskAppService.Complete(command);

            await taskRepositoryMock.Received().GetTaskByTitle(Arg.Is<string>(i => i == command.TaskTitle),
                Arg.Any<CancellationToken>());

            await taskRepositoryMock.Received().SaveAsync(Arg.Is<Domain.Tasks.Task>(t => t.Title == command.TaskTitle));
        }

        [Test]
        public void GetLatestFinished_Should_Throw_If_Filter_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.GetLatestFinisheds(null!));
        }

        [Test]
        public async Task GetLatestFinished()
        {
            var filter = new SearchTasksFilter(1, "authentication");

            var expectedTasks = new List<Domain.Tasks.Task>()
            {
                new()
                {
                    Title = "Feature authentication",
                    Description = "Implement authentication feature",
                    Branch = "feature/authentication",
                    Status = TaskStatus.InProgress
                }
            };

            taskRepositoryMock.GetLatestFinished(Arg.Any<CancellationToken>())
                .Returns(expectedTasks);

            var tasks = await taskAppService.GetLatestFinisheds(filter);

            Assert.That(tasks, Has.Count.EqualTo(expectedTasks.Count));

            await taskRepositoryMock.Received().GetLatestFinished(Arg.Any<CancellationToken>());
        }

        [Test]
        public void Update_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Update(null!));
        }

        [Test]
        public async Task Update()
        {
            var command = new UpdateTaskCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Description = "Description",
                Branch = "Branch"
            };

            var expectedTask = new Domain.Tasks.Task()
            {
                Id = command.Id,
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            taskRepositoryMock.GetTask(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(expectedTask);

            await taskAppService.Update(command);

            await taskRepositoryMock.Received().GetTask(Arg.Is<Guid>(i => i == command.Id),
                Arg.Any<CancellationToken>());

            await taskRepositoryMock.Received().SaveAsync(Arg.Is<Domain.Tasks.Task>(t => t.Id == expectedTask.Id
            && t.Title == command.Title && t.Description == command.Description && t.Branch == command.Branch ));
        }
    }
}
