using System.Data.SqlClient;

namespace Repository.IRepository
{
    public interface IUsuarioRepository<T> where T : class 
    {
        bool Create(T entity);
        void Update(T entity);
        bool Delete(int id);
        List<T> GetAll();
        T GetById(int id);
    }
}
