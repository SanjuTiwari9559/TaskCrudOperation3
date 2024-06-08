using System.ComponentModel.DataAnnotations;

namespace TaskCrudOperation3.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
