namespace ReactApp1.Server.DTOs
{
    public class UserPaginationDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? NameFilter { get; set; }
        public string? EmailFilter { get; set; }
        public int? IdFilter { get; set; }
    }
}
