namespace Repository.IRepository
{
    public interface IDisciplinaRepository<T> where T : class
    {
        // Cria uma nova disciplina no banco de dados.
        bool Create(T entity);
        // Atualiza uma disciplina no banco de dados.
        void Update(T entity);
        // Atualiza o status de uma disciplina no banco de dados.
        bool UpdateStatus(bool status, int disciplineId);
        // Exclui uma disciplina com base no ID.
        bool Delete(int id);
        // Exclui todas as disciplinas associadas a um professor com base no ID do professor.
        bool DeleteByIdTeacher(int teacherId);
        // Obtém todas as disciplinas associadas a um professor com base no ID do professor.
        List<T> GetAllByIdTeacher(int id);
        // Obtém todas as disciplinas de uma determinada série.
        List<T> GetAllBySerie(int id);
        // Obtém todas as disciplinas no banco de dados.
        List<T> GetAll();
        // Obtém uma disciplina com base no ID.
        T GetById(int id);

    }
}
