namespace ReactApp1.Server.Interface
{
    public interface IReadRepository<T> : IRepository<T> where T : IModelClass
    {
        new T GetById(int id);
        new IQueryable<T> GetAll();
    }
}
