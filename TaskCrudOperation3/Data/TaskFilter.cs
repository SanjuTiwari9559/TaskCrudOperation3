namespace TaskCrudOperation3.Data
{
    public class TaskFilter
    {
        //public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedTo { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
