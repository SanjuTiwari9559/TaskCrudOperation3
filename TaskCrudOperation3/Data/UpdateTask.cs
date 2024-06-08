using System.ComponentModel.DataAnnotations;

namespace TaskCrudOperation3.Data
{
    public class UpdateTask
    {
        public string TaskName { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string Priority { get; set; }
        public int? AssignedTo { get; set; } // Nullable for unassigned tasks
        [Required]
        public string Status { get; set; } // Status of the task
        public bool IsAccepted { get; set; } // Indicates if the task is accepted by the user
    }
}
