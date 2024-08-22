using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Models
{
    public class Address //: IEntity
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}
