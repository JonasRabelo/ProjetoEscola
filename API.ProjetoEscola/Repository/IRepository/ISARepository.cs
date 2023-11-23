namespace Repository.IRepository
{
    public interface ISARepository<T> where T : class
    {
        // Verifica se um superusuário com as credenciais fornecidas existe no sistema, e retorna verdadeiro (sim) ou false (não).
        bool Get(string login, string senha);
    }
}
