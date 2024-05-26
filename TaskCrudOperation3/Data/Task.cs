using System.ComponentModel.DataAnnotations;

namespace TaskCrudOperation3.Data
{
    public class Task
    {

        [Key]
        public int Id { get; set; } // Primary key
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public int? AssignedTo { get; set; } // Nullable for unassigned tasks
    }
}
