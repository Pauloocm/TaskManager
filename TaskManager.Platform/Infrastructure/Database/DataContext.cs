using Microsoft.EntityFrameworkCore;
using Task = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Platform.Infrastructure.Database
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

        public DbSet<Task> Task { get; set; }
    }
}
