﻿using Microsoft.Extensions.Logging;
using NSubstitute;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Tasks.Exceptions;
using TaskManager.Platform.Application;
using TaskManager.Platform.Application.Commands;
using Task = System.Threading.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Tests
{
    [TestFixture]
    public class TaskAppServiceTests
    {
        private ITaskRepository taskRepositoryMock = null!;
        private ILogger<TaskAppService> loggerMock = null!;
        private TaskAppService taskAppService = null!;

        [SetUp]
        public void Setup()
        {
            taskRepositoryMock = Substitute.For<ITaskRepository>();
            loggerMock = Substitute.For<ILogger<TaskAppService>>();
            taskAppService = new TaskAppService(taskRepositoryMock, loggerMock);
        }

        [Test]
        public void Add_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Add(null!));
        }

        [Test]
        public void Add_Should_Throw_TaskAlreadyExists_If_GetTaskByTitle_Return_Not_Null()
        {
            var command = new AddTaskCommand("title", "description", "branch", 1);

            var expectedTask = new Domain.Tasks.Task()
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            taskRepositoryMock.GetTaskByTitle(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(expectedTask);

            Assert.ThrowsAsync<TaskAlreadyExistsException>(async () => await taskAppService.Add(command));
        }

        [Test]
        public async Task Add()
        {
            var command = new AddTaskCommand("title", "description", "branch", 1);

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
        public void Complete_Should_Throw_TaskNotFound_If_GetTaskByTitle_Returns_Null()
        {
            var command = new CompleteTaskCommand(Guid.NewGuid());

            Assert.ThrowsAsync<TaskNotFoundException>(async () => await taskAppService.Complete(command));
        }

        [Test]
        public async Task Complete()
        {
            var taskId = Guid.NewGuid();
            var command = new CompleteTaskCommand(taskId);

            var expectedTask = new Domain.Tasks.Task()
            {
                Id = taskId,
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            taskRepositoryMock.GetTask(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(expectedTask);

            await taskAppService.Complete(command);

            await taskRepositoryMock.Received().GetTask(Arg.Is<Guid>(i => i == command.Id),
                Arg.Any<CancellationToken>());

            await taskRepositoryMock.Received().SaveAsync(Arg.Is<Domain.Tasks.Task>(t => t.Id == command.Id));
        }

        [Test]
        public async Task GetLatestFinished()
        {
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

            var tasks = await taskAppService.GetLatestFinisheds(CancellationToken.None);

            Assert.That(tasks, Has.Count.EqualTo(expectedTasks.Count));

            await taskRepositoryMock.Received().GetLatestFinished(Arg.Any<CancellationToken>());
        }

        [Test]
        public void Update_Should_Throw_If_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await taskAppService.Update(null!));
        }

        [Test]
        public void Update_Should_Throw_TaskNotFound_If_GetTask_Returns_Null()
        {
            var command = new UpdateTaskCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Description = "Description",
                Branch = "Branch"
            };

            Assert.ThrowsAsync<TaskNotFoundException>(async () => await taskAppService.Update(command));
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
            && t.Title == command.Title && t.Description == command.Description && t.Branch == command.Branch));
        }
    }
}
