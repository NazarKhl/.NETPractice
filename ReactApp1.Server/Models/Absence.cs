namespace ReactApp1.Server.Models
{
   public enum AbsenceType { 
      Illnes,
      Vacations, 
      Other
   }
    public class Absence
    {
        public int Id { get; set; }
        public AbsenceType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
