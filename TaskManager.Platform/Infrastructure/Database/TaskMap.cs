using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Tasks;
using Task = TaskManager.Domain.Tasks.Task;
using TaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Platform.Infrastructure.Database
{
    public class TaskMap : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);

            builder.Property(t => t.Description).IsRequired().HasMaxLength(500);

            builder.Property(t => t.Branch).IsRequired().HasMaxLength(50);

            builder.Property(t => t.Type).HasConversion(taskType => taskType.Id, type => TaskType.GetById(type)!);

            builder.Property(t => t.Status).HasConversion(taskStatus => taskStatus.Id, status => TaskStatus.GetById(status)!);

            builder.HasIndex(t => t.Title).IsUnique();

            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.UpdatedAt);
            builder.Property(t => t.CompletedAt);
        }
    }
}
