using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;
using TaskCrudOperation3.Data;
using Task = TaskCrudOperation3.Data.Task;

namespace TaskCrudOperation3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {// Controllers/TasksController.cs
        private readonly TaskDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskManagementController(TaskDbContext context, IEmailSender emailSender, IHubContext<TaskHub> hubContext)
        {
            _emailSender = emailSender;
            _context = context;
            _hubContext = hubContext;
        }

        // Get all tasks
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.tasks.ToListAsync();
            return Ok(tasks);
        }

        // Get task by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // Create a new task
        [HttpPost]
        public async Task<IActionResult> CreateTask(Task task)
        {
            _context.tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // Assign a task to a user
        [HttpPost("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignTask(int taskId, int userId)
        {
            var task = await _context.tasks.FindAsync(taskId);
            var user = await _context.users.FindAsync(userId);

            if (task == null || user == null)
            {
                return NotFound();
            }

            task.AssignedTo = user.Id;
            task.Status = "assigned";
            task.IsAccepted = false;
           
            await _context.SaveChangesAsync();

            await _emailSender.SendEmailAsync(user.Email, "New Task Assigned", $"Hi {user.Name}, you have been assigned a new task: {task.TaskName}.");
            //await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", $"Task \"{task.TaskName}\" has been assigned to {user.Name}.");

            return Ok(new { status = "success", message = "Task assigned and user notified." });
        }

        // Update task status
        [HttpPost("{taskId}/status/{status}")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, string status)
        {
            var task = await _context.tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task.Status = status;
            task.IsAccepted = status == "accepted";
            //task.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var user = await _context.users.FindAsync(task.AssignedTo);
            await _emailSender.SendEmailAsync(user.Email, $"Task {status}", $"Hi {user.Name}, the task \"{task.TaskName}\" has been {status}.");
            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", $"Task \"{task.TaskName}\" has been {status}.");

            return Ok(new { status = "success", message = $"Task {status} and user notified." });
        }
    }

}

