using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
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
        public  ActionResult<IEnumerable<Task>> GetTasks(int pageNumber = 1, int pageSize = 10)
        {
            var tasks1 = taskDbContext.tasks
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();
            return tasks1;
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
        public Responce PostTask(Task task)
        {
            taskDbContext.tasks.Add(task);
            taskDbContext.SaveChanges();

            return new Responce
            {
                Message ="Task Add Successfully"
            };
        }
        [HttpPut("{id}")]
        public Responce PutTask(int id, UpdateTask task)
        {
            var existTask = taskDbContext.tasks.Find(id);
            if (existTask == null)
            {
                return new Responce
                {
                    Message = "Task not assigned"
                };
            }
            existTask.TaskName = task.TaskName;
            existTask.Description = task.Description;
            existTask.DueDate = task.DueDate;
            existTask.Priority = task.Priority;
            existTask.AssignedTo = task.AssignedTo;
            existTask.Status = task.Status;
            existTask.IsAccepted = task.IsAccepted;
            taskDbContext.SaveChanges();
            return new Responce
            {
                Message = "Task Update Succesfully"
            };
        }
           


        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public Responce DeleteTask(int id)
        {
            var task = taskDbContext.tasks.Find(id);
            if (task == null)
            {
                return new Responce
                {
                    Message = "Id Not found"
                };
            }

            taskDbContext.tasks.Remove(task);
            taskDbContext.SaveChanges();

            return new Responce
            {
                Message="Delete Task Successfully"
            };
        }

       
    }

}

