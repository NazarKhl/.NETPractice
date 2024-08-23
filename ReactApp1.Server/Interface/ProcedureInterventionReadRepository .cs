using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using System.Linq;

namespace ReactApp1.Server.Interface
{
    public class ProcedureInterventionReadRepository : IProcedureInterventionRepository<ProcedureInterventionModel>
    {
        private readonly UserDBContext _userDBContext;

        public ProcedureInterventionReadRepository(UserDBContext userDBContext)
        {
            _userDBContext = userDBContext;
        }

        public ProcedureInterventionModel GetById(int id)
        {
            return _userDBContext.ProcedureInterventions
                .FromSqlInterpolated($"EXEC dbo.GetInterventionsForCustomer @CustomerId = {id}, @Date = {DateTime.Now:yyyy-MM-dd}")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public IQueryable<ProcedureInterventionModel> GetAll(int customerId, DateTime date)
        {
            return _userDBContext.ProcedureInterventions
                .FromSqlInterpolated($"EXEC dbo.GetInterventionsForCustomer @CustomerId = {customerId}, @Date = {date:yyyy-MM-dd}")
                .AsQueryable();
        }
    }
}
