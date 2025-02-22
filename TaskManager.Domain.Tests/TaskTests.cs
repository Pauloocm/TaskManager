﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Internal;
using TaskManager.Domain.Tasks;
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
                Status = TaskStatus.InProgress,
                Type = TaskType.Feature
            };

            Assert.That(task.CompletedAt, Is.Null);

            task.Complete();

            Assert.Multiple(() =>
            {

                Assert.That(task.Title, Is.EqualTo("Feature authentication"));
                Assert.That(task.Description, Is.EqualTo("Implement authentication feature"));
                Assert.That(task.Branch, Is.EqualTo("feature/authentication"));
                Assert.That(task.Type, Is.EqualTo(TaskType.Feature));
                Assert.That(task.Status, Is.EqualTo(TaskStatus.Done));
                Assert.That(task?.CompletedAt, Is.Not.Null);
                Assert.That(task?.CompletedAt!.Value.Date, Is.Not.EqualTo(DateTime.MinValue));
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

            var test1 = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");
            var test2 = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

            var test3 = DateTime.Now.ToUniversalTime();
            var test4 = test3.ToLocalTime();
            var test5 = test4.ToLocalTime();

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
