using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using System;
using System.Linq;

namespace ReactApp1.Server.Interface
{
    public class HoursInterventionRepository : IHoursInterventionRepository<HoursInterventionModel>
    {
        private readonly UserDBContext _userDBContext;

        public HoursInterventionRepository(UserDBContext userDBContext)
        {
            _userDBContext = userDBContext;
        }

        public HoursInterventionModel GetById(int id)
        {
            return _userDBContext.HoursInterventionModels
                .FromSqlInterpolated($"EXEC dbo.GetTotalHou @UserId = {id}, @CustomerId = NULL, @YearMonth = NULL")
                .AsEnumerable()
                .FirstOrDefault();
        }

        public IQueryable<HoursInterventionModel> GetAll(int? userId, int? customerId, DateTime? date)
        {
            var yearMonthStr = date?.ToString("yyyy-MM") ?? null;

            return _userDBContext.HoursInterventionModels
                .FromSqlInterpolated($"EXEC dbo.GetTotalHou @UserId = {userId}, @CustomerId = {customerId}, @YearMonth = {yearMonthStr}")
                .AsQueryable();
        }
    }
}
