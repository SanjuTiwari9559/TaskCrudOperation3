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
                Message ="Task Add S"
            };
        }
        [HttpPut("{id}")]
        public Responce PutTask(int id, Task task)
        {
            // Check if the provided id matches the task id
            if (id != task.Id)
            {
                return new Responce
                {
                    Message = "Task ID mismatch"
                };
            }

            try
            {
                // Assuming taskDbContext is an instance of TaskDbContext and is properly initialized
                var existingTask = taskDbContext.tasks.Find(id);
           

                if (existingTask == null)
                {
                    return new Responce
                    {
                        Message = "Task not found"
                    };
                }

                // Update the task
                existingTask.TaskName = task.TaskName;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.AssignedTo = task.AssignedTo;
                taskDbContext.tasks.Update(existingTask);
                taskDbContext.SaveChanges();

                return new Responce
                {
                    Message = "Task updated successfully"
                };
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                return new Responce
                {
                    Message = $"An error occurred while updating the task: {ex.Message}"
                };
            }
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

