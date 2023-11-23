namespace Repository.IRepository
{
    public interface IMatriculaRepository<T> where T : class
    {
        bool Create(T entity);
        void Update(T entity);
        bool Delete(int id);
        bool DeleteByIdDiscipline(int disciplineId);
        bool DeleteByIdStudent(int studentId);
        List<T> GetAllByIdStudent(int id);
        List<T> GetAllByIdDiscipline(int id);
        List<T> GetAll();
        T GetById(int id);
    }
}
