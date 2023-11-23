namespace Repository.IRepository
{
    public interface IMatriculaRepository<T> where T : class
    {
        // Cria uma nova entidade de matrícula e retorna se a operação foi bem-sucedida.
        bool Create(T entity);

        // Atualiza uma entidade de matrícula existente.
        void Update(T entity);

        // Exclui uma entidade de matrícula com base no ID e retorna se a operação foi bem-sucedida.
        bool Delete(int id);

        // Exclui entidades de matrícula com base no ID da disciplina associada e retorna se a operação foi bem-sucedida.
        bool DeleteByIdDiscipline(int disciplineId);

        // Exclui entidades de matrícula com base no ID do aluno associado e retorna se a operação foi bem-sucedida.
        bool DeleteByIdStudent(int studentId);

        // Obtém uma lista de todas as entidades de matrícula associadas a um determinado ID de aluno.
        List<T> GetAllByIdStudent(int id);

        // Obtém uma lista de todas as entidades de matrícula associadas a um determinado ID de disciplina.
        List<T> GetAllByIdDiscipline(int id);

        // Obtém uma lista de todas as entidades de matrícula.
        List<T> GetAll();

        // Obtém uma única entidade de matrícula com base no ID fornecido.
        T GetById(int id);
    }
}
