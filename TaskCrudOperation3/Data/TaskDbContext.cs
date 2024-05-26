using Microsoft.EntityFrameworkCore;

namespace TaskCrudOperation3.Data
{
    public class TaskDbContext:DbContext
    {
        private readonly DbContextOptions<TaskDbContext> option;

        public TaskDbContext(DbContextOptions<TaskDbContext>_option):base(_option)
        {
            option = _option;
        }
        public DbSet<Task> tasks { get; set; }
    }
}
