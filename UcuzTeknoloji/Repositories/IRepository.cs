namespace UcuzTeknoloji.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        void Add(T entity);
        void Delete(int id);
        void Save();
    }
}