using System.Collections.Generic;
namespace ReactApp1.Server.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public int PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public List<InterventionDTO> Interventions { get; set; }
        public List<AddressDTO> Addresses { get; set; }
    }

    public class InterventionDTO
    {
        public int Id { get; set; }
    }

    public class AddressDTO
    {
        public int Id { get; set; }
    }
}
