using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Data
{
    public class ReadRepository<T> : IReadRepository<T> where T : class, IModelClass
    {
        private readonly UserDBContext _context;
        private readonly DbSet<T> _dbSet;

        public ReadRepository(UserDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(T entity)
        {
            throw new NotSupportedException("Add operation is not supported in ReadRepository.");
        }

        public void Update(T entity)
        {
            throw new NotSupportedException("Update operation is not supported in ReadRepository.");
        }

        public void Delete(T entity)
        {
            throw new NotSupportedException("Delete operation is not supported in ReadRepository.");
        }
    }
}
