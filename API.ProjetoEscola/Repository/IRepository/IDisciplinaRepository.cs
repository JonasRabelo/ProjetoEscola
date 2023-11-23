namespace Repository.IRepository
{
    public interface IDisciplinaRepository<T> where T : class
    {
        bool Create(T entity);
        void Update(T entity);
        bool UpdateStatus(bool status, int disciplineId);
        bool Delete(int id);
        bool DeleteByIdTeacher(int teacherId);
        List<T> GetAllByIdTeacher(int id);
        List<T> GetAllBySerie(int id);
        List<T> GetAll();
        T GetById(int id);

    }
}
