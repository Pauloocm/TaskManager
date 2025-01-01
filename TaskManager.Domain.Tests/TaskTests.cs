using Task = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Domain.Tests
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void Complete()
        {
            var task = new Task()
            {
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            Assert.That(task.CompletedAt, Is.Null);

            task.Complete();

            Assert.Multiple(() =>
            {
                Assert.That(task.Status, Is.EqualTo(TaskStatus.Done));
                Assert.That(task?.CompletedAt, Is.Not.Null);
                Assert.That(task?.CompletedAt!.Value.Date, Is.EqualTo(DateTime.Now.Date));
            });
        }
    }
}
