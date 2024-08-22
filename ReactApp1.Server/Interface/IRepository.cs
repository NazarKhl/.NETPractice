using ReactApp1.Server.Models.Abstractions;

namespace ReactApp1.Server.Interface
{
    public interface IRepository<T> : IReadRepository<T> where T : class//, IEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
