namespace ReactApp1.Server.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
        public List<AbsenceDTO> Absences { get; set; }
    }
}
