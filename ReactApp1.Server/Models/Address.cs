namespace ReactApp1.Server.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int InterventionId { get; set; }
        public virtual Intervention Intervention { get; set; }
    }
}
