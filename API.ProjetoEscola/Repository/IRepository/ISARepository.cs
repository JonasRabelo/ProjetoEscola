namespace Repository.IRepository
{
    public interface ISARepository<T> where T : class
    {
        bool Get(string login, string senha);
    }
}
