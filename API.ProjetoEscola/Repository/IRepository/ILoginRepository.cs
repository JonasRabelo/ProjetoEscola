using Models;

namespace Repository.IRepository
{
    public interface ILoginRepository<T> where T : class
    {
        // Obtém um professor com base nas informações de login fornecidas.
        ProfessorModel GetTeacherByLogin(T entity);
        // Obtém um aluno com base nas informações de login fornecidas.
        AlunoModel GetStudentByLogin(T entity);
    }
}
