using System;
using System.Linq;

namespace ReactApp1.Server.Interface
{
    public interface IHoursInterventionRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll(int userId, DateTime yearMonth);
    }
}
