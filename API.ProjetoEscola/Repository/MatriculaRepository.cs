using Models.Enums;
using Models;
using System.Data.SqlClient;
using Repository.IRepository;

namespace Repository
{
    /// <summary>
    /// Fornece funcionalidades de acesso a dados para gerenciar registros de matrícula em um banco de dados.
    /// Implementa a interface IMatriculaRepository para lidar com operações relacionadas a matrículas.
    /// </summary>
    public class MatriculaRepository : IMatriculaRepository<MatriculaModel>
    {
        private readonly string cs = string.Empty;

        public MatriculaRepository(string connectionString)
        {
            cs = connectionString;
        }

        /// <summary>
        /// Insere um novo registro de matrícula no banco de dados.
        /// </summary>
        /// <param name="entity">Uma instância de MatriculaModel representando os dados da matrícula a serem inseridos.</param>
        /// <returns>Verdadeiro se a inserção for bem-sucedida; caso contrário, falso.</returns>
        public bool Create(MatriculaModel entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Matriculas (nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula) VALUES (@nota1, @nota2, @nota3, @nota4, @mediaFinal, @alunoId, @disciplinaId, @dataMatricula)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@nota1", entity.Nota1);
                    cmd.Parameters.AddWithValue("@nota2", entity.Nota2);
                    cmd.Parameters.AddWithValue("@nota3", entity.Nota3);
                    cmd.Parameters.AddWithValue("@nota4", entity.Nota4);
                    cmd.Parameters.AddWithValue("@mediaFinal", (entity.Nota1 + entity.Nota2 + entity.Nota3 + entity.Nota4) / 4);
                    cmd.Parameters.AddWithValue("@alunoId", entity.AlunoId);
                    cmd.Parameters.AddWithValue("@disciplinaId", entity.DisciplinaId);
                    cmd.Parameters.AddWithValue("dataMatricula", DateTime.Now);

                    cmd.Connection.Open();

                    return cmd.ExecuteNonQuery() > 0;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepository.Create: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui um registro de matrícula do banco de dados com base no ID da matrícula fornecido.
        /// </summary>
        /// <param name="id">O ID da matrícula a ser excluída.</param>
        /// <returns>Verdadeiro se a exclusão for bem-sucedida; caso contrário, falso.</returns>
        public bool Delete(int id)
        {
            string query = "DELETE FROM Matriculas WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id", id);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepository.Delete: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui todos os registros de matrícula associados a um ID de disciplina específico.
        /// </summary>
        /// <param name="disciplineId">O ID da disciplina para a qual os registros de matrícula correspondentes devem ser excluídos.</param>
        /// <returns>Verdadeiro se a exclusão for bem-sucedida; caso contrário, falso.</returns>
        public bool DeleteByIdDiscipline(int disciplineId)
        {
            string query = "DELETE FROM Matriculas WHERE disciplinaId = @disciplineId";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@disciplineId", disciplineId);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepository.DeleteByIdDiscipline: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui todos os registros de matrícula associados a um ID de aluno específico.
        /// </summary>
        /// <param name="studentId">O ID do aluno para o qual os registros de matrícula devem ser excluídos.</param>
        /// <returns>Verdadeiro se a exclusão for bem-sucedida; caso contrário, falso.</returns>
        public bool DeleteByIdStudent(int studentId)
        {
            string query = "DELETE FROM Matriculas WHERE alunoId = @studentId";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@studentId", studentId);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepository.DeleteByIdStudent: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Recupera uma lista de todos os registros de matrícula do banco de dados.
        /// </summary>
        /// <returns>Uma lista de MatriculaModel contendo todos os registros de matrícula.</returns>
        public List<MatriculaModel> GetAll()
        {
            List<MatriculaModel> matriculas = new List<MatriculaModel>();
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MatriculaModel matricula = new MatriculaModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nota1 = Convert.ToDouble(reader["nota1"]),
                            Nota2 = Convert.ToDouble(reader["nota2"]),
                            Nota3 = Convert.ToDouble(reader["nota3"]),
                            Nota4 = Convert.ToDouble(reader["nota4"]),
                            MediaFinal = Convert.ToDouble(reader["mediaFinal"]),
                            AlunoId = Convert.ToInt32(reader["alunoId"]),
                            DisciplinaId = Convert.ToInt32(reader["disciplinaId"]),
                            DataMatricula = DateTime.Parse(reader["dataMatricula"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") matricula.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        matriculas.Add(matricula);
                    }

                }
                return matriculas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepositry.GetAll: {ex.Message}");
            }
            return matriculas;
        }


        /// <summary>
        /// Recupera uma lista de registros de matrícula associados a um ID de disciplina específico.
        /// </summary>
        /// <param name="id">O ID da disciplina para a qual os registros de matrícula correspondentes devem ser recuperados.</param>
        /// <returns>Uma lista de MatriculaModel contendo registros de matrícula associados ao ID de disciplina especificado.</returns>
        public List<MatriculaModel> GetAllByIdDiscipline(int id)
        {
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas WHERE disciplinaId = @id";
            return GetAll(query, id);
        }


        /// <summary>
        /// Recupera uma lista de registros de matrícula associados a um ID de aluno específico.
        /// </summary>
        /// <param name="id">O ID do aluno para o qual os registros de matrícula devem ser recuperados.</param>
        /// <returns>Uma lista de MatriculaModel contendo registros de matrícula associados ao ID de aluno especificado.</returns>
        public List<MatriculaModel> GetAllByIdStudent(int id)
        {
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas WHERE alunoId = @id";
            return GetAll(query, id);
        }


        /// <summary>
        /// Recupera um objeto MatriculaModel com base no ID da matrícula fornecido.
        /// </summary>
        /// <param name="id">O ID da matrícula a ser recuperada.</param>
        /// <returns>Um objeto MatriculaModel representando o registro de matrícula correspondente ao ID fornecido.</returns>
        public MatriculaModel GetById(int id)
        {
            MatriculaModel matricula = new MatriculaModel();
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas WHERE Id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id", id);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        matricula.Id = Convert.ToInt32(reader["Id"]);
                        matricula.Nota1 = Convert.ToDouble(reader["nota1"]);
                        matricula.Nota2 = Convert.ToDouble(reader["nota2"]);
                        matricula.Nota3 = Convert.ToDouble(reader["nota3"]);
                        matricula.Nota4 = Convert.ToDouble(reader["nota4"]);
                        matricula.MediaFinal = Convert.ToDouble(reader["mediaFinal"]);
                        matricula.AlunoId = Convert.ToInt32(reader["alunoId"]);
                        matricula.DisciplinaId = Convert.ToInt32(reader["disciplinaId"]);
                        matricula.DataMatricula = DateTime.Parse(reader["dataMatricula"].ToString()!);
                        if (reader["dataMatricula"].ToString().Length > 0) matricula.DataMatricula = DateTime.Parse(reader["dataMatricula"].ToString()!); matricula.DataMatricula = DateTime.Parse(reader["dataMatricula"].ToString()!);
                        if (reader["dataDeAtualizacao"].ToString().Length > 0) matricula.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!); matricula.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                    }
                }
                return matricula;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepositry.GetById: {ex.Message}");
            }
            return matricula;
        }


        /// <summary>
        /// Atualiza os dados de um registro de matrícula no banco de dados.
        /// </summary>
        /// <param name="entity">Uma instância de MatriculaModel contendo os dados atualizados da matrícula.</param>
        public void Update(MatriculaModel entity)
        {
            string query = @"UPDATE Matriculas SET nota1 = @nota1, nota2 = @nota2, nota3 = @nota3, nota4 = @nota4, mediaFinal = @mediaFinal, dataDeAtualizacao = @dataDeAtualizacao WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@nota1", entity.Nota1);
                    cmd.Parameters.AddWithValue("@nota2", entity.Nota2);
                    cmd.Parameters.AddWithValue("@nota3", entity.Nota3);
                    cmd.Parameters.AddWithValue("@nota4", entity.Nota4);
                    cmd.Parameters.AddWithValue("@mediaFinal", (entity.Nota1 + entity.Nota2 + entity.Nota3 + entity.Nota4) / 4);
                    cmd.Parameters.AddWithValue("dataDeAtualizacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("id", entity.Id);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepository.Update: {ex.Message}");
            }
        }

        // Método privado auxiliar para recuperar registros com base em um query específico e um ID fornecido.
        private List<MatriculaModel> GetAll(string query, int id)
        {
            List<MatriculaModel> matriculas = new List<MatriculaModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MatriculaModel matricula = new MatriculaModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nota1 = Convert.ToDouble(reader["nota1"]),
                            Nota2 = Convert.ToDouble(reader["nota2"]),
                            Nota3 = Convert.ToDouble(reader["nota3"]),
                            Nota4 = Convert.ToDouble(reader["nota4"]),
                            MediaFinal = Convert.ToDouble(reader["mediaFinal"]),
                            AlunoId = Convert.ToInt32(reader["alunoId"]),
                            DisciplinaId = Convert.ToInt32(reader["disciplinaId"]),
                            DataMatricula = DateTime.Parse(reader["dataMatricula"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") matricula.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        matriculas.Add(matricula);
                    }

                }
                return matriculas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MatriculaRepositry.GetAll(id): {ex.Message}");
            }
            return matriculas;
        }
    }
}
