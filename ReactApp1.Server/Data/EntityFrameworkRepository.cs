using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Interface;

namespace ReactApp1.Server.Data
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private readonly UserDBContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EntityFrameworkRepository(UserDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll() 
        {
            return _dbSet.ToList();
        }

        public void Add(T entity) 
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        { 
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified; 
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
