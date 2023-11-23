using Models;

namespace Repository.IRepository
{
    public interface ILoginRepository<T> where T : class
    {
        ProfessorModel GetTeacherByLogin(T entity);
        AlunoModel GetStudentByLogin(T entity);
        bool UpdatePassword(T entity);
    }
}
