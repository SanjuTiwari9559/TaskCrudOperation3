using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TaskCrudOperation3.Data;
using Task = TaskCrudOperation3.Data.Task;

namespace TaskCrudOperation3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskDbContext taskDbContext;

        public TaskController(TaskDbContext taskDbContext)
        {
            this.taskDbContext = taskDbContext;
        }
        [HttpGet]
        public  ActionResult<IEnumerable<Task>> GetTasks()
        {
            return taskDbContext.tasks.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            var task = taskDbContext.tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }
        [HttpPost]
        public ActionResult<Task> PostTask(Task task)
        {
            taskDbContext.tasks.Add(task);
            taskDbContext.SaveChanges();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        [HttpPut("{id}")]
        public IActionResult PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            taskDbContext.Entry(task).State = EntityState.Modified;

            try
            {
                taskDbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = taskDbContext.tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            taskDbContext.tasks.Remove(task);
            taskDbContext.SaveChanges();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return taskDbContext.tasks.Any(e => e.Id == id);
        }
    }

}

