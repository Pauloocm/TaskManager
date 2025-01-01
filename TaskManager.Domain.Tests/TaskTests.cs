using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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

        [TestCase("", "description", "branch")]
        [TestCase("title", "", "branch")]
        [TestCase("title", "description", "")]
        public void Update_Should_Throw_If_Parameters_Are_Null(string title, string description, string branch)
        {
            var task = new Task()
            {
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            Assert.Throws<ArgumentException>(() => task.Update(title, description, branch));

            Assert.Multiple(() =>
            {
                Assert.That(task.Title, Is.EqualTo("Feature authentication"));
                Assert.That(task.Description, Is.EqualTo("Implement authentication feature"));
                Assert.That(task.Branch, Is.EqualTo("feature/authentication"));
            });
        }

        [Test]
        public void Update()
        {
            var task = new Task()
            {
                Title = "Feature authentication",
                Description = "Implement authentication feature",
                Branch = "feature/authentication",
                Status = TaskStatus.InProgress
            };

            var newTitle = "Feature authorization Updated";
            var newDescription = "Implement Update";
            var newBranch = "hotfix/authorization";

            task.Update(newTitle, newDescription, newBranch);

            Assert.Multiple(() =>
            {
                Assert.That(task.Title, Is.EqualTo(newTitle));
                Assert.That(task.Description, Is.EqualTo(newDescription));
                Assert.That(task.Branch, Is.EqualTo(newBranch));
            });
        }
    }
}
