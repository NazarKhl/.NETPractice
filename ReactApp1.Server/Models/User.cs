using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }

        public virtual List<Absence> Absences { get; set; } = new List<Absence>();
        public virtual List<Intervention> Interventions { get; set; } = new List<Intervention>();
    }

}
