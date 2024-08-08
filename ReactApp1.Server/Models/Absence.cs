namespace ReactApp1.Server.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AbsenceType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public virtual User User { get; set; }

        public enum AbsenceType
        {
            Illnes,
            Vacation,
            Other
        }
    }
}
