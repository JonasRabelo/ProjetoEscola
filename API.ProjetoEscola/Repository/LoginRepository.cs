using Models;
using Models.Enums;
using Repository.IRepository;
using System.Data.SqlClient;
using System.Drawing;

namespace Repository
{
    public class LoginRepository : ILoginRepository<LoginModel>
    {
        private readonly string cs = string.Empty;

        public LoginRepository(string connectionString)
        {
            cs = connectionString;
        }


        /// <summary>
        /// Obtém um aluno com base no login fornecido.
        /// </summary>
        /// <param name="entity">Modelo contendo informações de login.</param>
        /// <returns>Um objeto AlunoModel se encontrado, ou um objeto AlunoModel vazio se não encontrado.</returns>
        public AlunoModel GetStudentByLogin(LoginModel entity)
        {
            AlunoModel aluno = new AlunoModel();
            string query = "SELECT Id, nome, login, senha, email, serie, dataDeCadastro, dataDeAtualizacao FROM Alunos WHERE login = @login";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", entity.Login);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        aluno.Id = Convert.ToInt32(reader["Id"]);
                        aluno.Nome = reader["nome"].ToString()!;
                        aluno.Login = reader["login"].ToString()!;
                        aluno.Senha = reader["senha"].ToString()!;
                        aluno.Email = reader["email"].ToString()!;
                        aluno.Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!);
                        aluno.DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!);
                        if (reader["dataDeAtualizacao"].ToString()! != "") aluno.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                    }
                    return aluno;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginRepository.GetStudentByLogin: {ex.Message}");
            }
            return aluno;
        }


        /// <summary>
        /// Obtém um professor com base no login fornecido.
        /// </summary>
        /// <param name="entity">Modelo contendo informações de login.</param>
        /// <returns>Um objeto ProfessorModel se encontrado, ou um objeto ProfessorModel vazio se não encontrado.</returns>
        public ProfessorModel GetTeacherByLogin(LoginModel entity)
        {
            ProfessorModel professor = new ProfessorModel();
            string query = "SELECT Id, nome, login, senha, email, dataDeCadastro, dataDeAtualizacao FROM Professores WHERE login = @login";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", entity.Login);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        professor.Id = Convert.ToInt32(reader["Id"]);
                        professor.Nome = reader["nome"].ToString()!;
                        professor.Login = reader["login"].ToString()!;
                        professor.Senha = reader["senha"].ToString()!;
                        professor.Email = reader["email"].ToString()!;
                        professor.DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!);
                        if (reader["dataDeAtualizacao"].ToString()! != "") professor.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                    }
                    return professor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginRepository.GetTeacherByLogin: {ex.Message}");
            }
            return professor;
        }
    }
}
