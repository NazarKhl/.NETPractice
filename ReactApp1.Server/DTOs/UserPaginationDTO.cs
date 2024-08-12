namespace ReactApp1.Server.DTOs
{
    public class UserPaginationDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortColumn { get; set; } = "id";
        public string? SortDirection { get; set; } = "asc";
        public string? NameFilter { get; set; }
        public string? EmailFilter { get; set; }
        public int? IdFilter { get; set; }
    }
}
