namespace ReactApp1.Server.Models
{
    public class ProcedureInterventionModel
    {
        public int InterventionId { get; set; }
        public string InterventionDescription { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }
    }
}
