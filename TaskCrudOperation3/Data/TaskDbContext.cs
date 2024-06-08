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
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Task entity
            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Tasks"); // Explicitly specify table name
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TaskName).IsRequired();
                entity.Property(e => e.Description); // Configure Description property
                entity.Property(e => e.DueDate).IsRequired();
                entity.Property(e => e.Priority).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.IsAccepted).IsRequired();

                // Configure relationship with User
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.AssignedTo)
                      .OnDelete(DeleteBehavior.SetNull); // Nullable foreign key
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users"); // Explicitly specify table name
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });
        }
    }
}
