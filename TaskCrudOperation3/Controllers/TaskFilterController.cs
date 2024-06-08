using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
                query = ApplySort(query, filter.SortBy, filter.SortOrder);
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
        private IQueryable<Task> ApplySort(IQueryable<Task> source, string sortBy, string sortOrder)
        {
            var propertyInfo = typeof(Task).GetProperty(sortBy);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"Property '{sortBy}' does not exist on type '{typeof(Task)}'.");
            }

            var parameter = Expression.Parameter(typeof(Task), "e");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                                                   new Type[] { typeof(Task), propertyInfo.PropertyType },
                                                   source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<Task>(resultExpression);
        }
    }
}

        public class PagedResult<T>
        {
            public IEnumerable<T> Items { get; set; }
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

    

