namespace ReactApp1.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
        public List<Absence> Absences { get; set; }
    }

}
