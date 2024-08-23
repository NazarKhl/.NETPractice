namespace ReactApp1.Server.Models
{
    public class HoursInterventionModel
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public int TotalHoursOfWork { get; set; }
    }
}
