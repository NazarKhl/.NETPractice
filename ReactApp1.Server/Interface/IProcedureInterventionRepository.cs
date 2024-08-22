using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Interface
{
    public interface IProcedureInterventionRepository<T> where T : class //, IEntity
    {
        T GetById(int id);
        IQueryable<T> GetAll(int customerId, DateTime date);
    }
}
