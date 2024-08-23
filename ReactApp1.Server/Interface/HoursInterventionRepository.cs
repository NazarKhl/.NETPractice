using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Data
{
    public class HoursInterventionRepository : IHoursInterventionRepository<HoursInterventionModel>
    {
        private readonly UserDBContext _context;

        public HoursInterventionRepository(UserDBContext context)
        {
            _context = context;
        }

        public HoursInterventionModel GetById(int id)
        {

            return _context.HoursInterventionModels
                .SingleOrDefault(e => e.UserId == id);
        }

        public IQueryable<HoursInterventionModel> GetAll(int userId, DateTime yearMonth)
        {
            var startDate = new DateTime(yearMonth.Year, yearMonth.Month, 1);
            var endDate = startDate.AddMonths(1);

            return _context.Intervention
                .Where(i => i.UserId == userId && i.Date >= startDate && i.Date < endDate)
                .GroupBy(i => new { i.UserId })
                .Select(g => new HoursInterventionModel
                {
                    UserId = g.Key.UserId,
                    TotalHoursOfWork = g.Sum(i => i.HoursOfWork)
                })
                .AsQueryable();
        }
    }
}
