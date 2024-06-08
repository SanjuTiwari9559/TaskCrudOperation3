using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskCrudOperation3.Data;
using TaskStatus = TaskCrudOperation3.Data.TaskStatus;

namespace TaskCrudOperation3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateTaskStatusController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public UpdateTaskStatusController(TaskDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateTaskStatus(int assignedTo, [FromBody] string newStatus)
        {
            var tasks = _context.tasks.Where(t => t.AssignedTo == assignedTo).ToList();

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound("No tasks assigned to the specified user.");
            }

            foreach (var task in tasks)
            {
                task.Status = newStatus;
            }
            _context.SaveChanges();

            return Ok(tasks);
        }
    }
}

