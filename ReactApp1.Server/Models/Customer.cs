using System.Net;

namespace ReactApp1.Server.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public int PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public virtual List<Intervention> Interventions { get; set; } = new List<Intervention>();
        public virtual List<Address> Addresses { get; set; } = new List<Address>();
    }
}
