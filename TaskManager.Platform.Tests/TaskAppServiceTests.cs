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
            var command = new AddTaskCommand("title", "description", "branch");

            var expectedTask = new Domain.Tasks.Task()
            {
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            var id = await taskAppService.Add(command);

            await taskRepositoryMock.Received().Add(Arg.Is<Domain.Tasks.Task>(t =>
            t.Title == command.Title && t.Description == command.Description && t.Branch == command.Branch));

            await taskRepositoryMock.Received().Commit();
        }

        [Test]
        public void Complete_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Complete(null!));
        }

        [Test]
        public async Task Complete()
        {
            var command = new CompleteTaskCommand(Guid.NewGuid());

            var expectedTask = new Domain.Tasks.Task()
            {
                Id = command.Id,
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            taskRepositoryMock.GetTask(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(expectedTask);

            await taskAppService.Complete(command);

            await taskRepositoryMock.Received().GetTask(Arg.Is<Guid>(i => i == command.Id),
                Arg.Any<CancellationToken>());

            await taskRepositoryMock.Received().Commit();
        }

        [Test]
        public void Search_Should_Throw_If_Filter_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Search(null!));
        }

        [Test]
        public async Task Search()
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

            taskRepositoryMock.SearchTasks(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(expectedTasks);

            var tasks = await taskAppService.Search(filter);
            
            Assert.That(tasks, Has.Count.EqualTo(expectedTasks.Count));

            await taskRepositoryMock.Received().SearchTasks(Arg.Is<string>(t => t == filter.Term),
                Arg.Is<int>(p => p == filter.Page), Arg.Any<CancellationToken>());
        }

        [Test]
        public void Update_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Update(null!));
        }

        [Test]
        public async Task Update()
        {
            var command = new UpdateTaskCommand(Guid.NewGuid(), "title", "description", "branch");

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

            await taskRepositoryMock.Received().Commit(Arg.Any<CancellationToken>());
        }
    }
}
