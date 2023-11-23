using System.Data.SqlClient;

namespace Repository.IRepository
{
    public interface IUsuarioRepository<T> where T : class 
    {
        // Cria uma nova entidade de usuário no sistema.
        bool Create(T entity);
        // Atualiza os detalhes de uma entidade de usuário existente.
        void Update(T entity);
        // Exclui uma entidade de usuário com base no ID.
        bool Delete(int id);
        // Obtém todas as entidades de usuário no sistema.
        List<T> GetAll();
        // Obtém uma entidade de usuário com base no ID.
        T GetById(int id);
    }
}
