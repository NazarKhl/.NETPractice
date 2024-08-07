namespace ReactApp1.Server.DTOs
{
    public class AbsenceDTO
    {
        public enum AbsenceType
        {
            Illnes,
            Vacations,
            Other
        }

        public int Id { get; set; }
        public AbsenceType Type { get; set; }
        public string Description { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
