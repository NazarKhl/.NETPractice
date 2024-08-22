using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Models
{
    public class Intervention// : IEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int AddressId { get; set; }
        public virtual Address Addresses { get; set; }

    }
}
