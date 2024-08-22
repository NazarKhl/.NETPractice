using ReactApp1.Server.Data;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Interface
{
    public class MonthlyInterventionReadRepository : IReadRepository<MonthlyInterventionModel>
    {
        public readonly UserDBContext _userDBContext;

        public MonthlyInterventionReadRepository(UserDBContext userDBContext)
        {
            _userDBContext = userDBContext;
        }
        public MonthlyInterventionModel GetById(int id)
        {
            return _userDBContext.MonthlyInterventions
                .Where(m => m.InterventionId == id)
                .FirstOrDefault();
        }
        public IQueryable<MonthlyInterventionModel> GetAll()
        {
            return _userDBContext.MonthlyInterventions.AsQueryable();
        }
    }
}
