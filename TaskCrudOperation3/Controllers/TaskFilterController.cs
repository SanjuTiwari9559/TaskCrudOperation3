using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskCrudOperation3.Data;
using Task = TaskCrudOperation3.Data.Task;

namespace TaskCrudOperation3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    
        public class TasksFilter : ControllerBase
        {
            private readonly TaskDbContext _context;

        public TasksFilter(TaskDbContext context)
        {
            this._context = context;

        }
        [HttpGet]
            public async Task<ActionResult<PagedResult<Task>>> GetTasks([FromQuery] TaskFilter filter)
            {
                IQueryable<Task> query = _context.tasks;

                // Filtering
                //if (!string.IsNullOrEmpty(filter.Status))
                //{
                //    query = query.Where(t => t.Status == filter.Status);
                //}
                if (filter.DueDate.HasValue)
                {
                    query = query.Where(t => t.DueDate.Date == filter.DueDate.Value.Date);
                }
                if (filter.AssignedTo.HasValue)
                {
                    query = query.Where(t => t.AssignedTo == filter.AssignedTo);
                }

                // Sorting
                if (!string.IsNullOrEmpty(filter.SortBy))
                {
                    if (filter.SortOrder.ToLower() == "desc")
                    {
                        query = query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy));
                    }
                    else
                    {
                        query = query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
                    }
                }

                // Pagination
                int totalRecords = await query.CountAsync();
                List<Task> tasks = await query.Skip((filter.PageNumber - 1) * filter.PageSize)
                                              .Take(filter.PageSize)
                                              .ToListAsync();

                var pagedResult = new PagedResult<Task>
                {
                    Items = tasks,
                    TotalCount = totalRecords,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };

                return Ok(pagedResult);
            }
        }

        public class PagedResult<T>
        {
            public IEnumerable<T> Items { get; set; }
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

    }

