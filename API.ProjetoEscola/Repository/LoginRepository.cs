using Models;
using Models.Enums;
using Repository.IRepository;
using System.Data.SqlClient;

namespace Repository
{
    public class LoginRepository : ILoginRepository<LoginModel>
    {
        private readonly string cs = "server=DESKTOP-MQADPEC\\SQLEXPRESS; database=DB_EscolaMJV; Trusted_Connection = true; Integrated Security=SSPI;TrustServerCertificate=True";

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

        public bool UpdatePassword(LoginModel entity)
        {
            string queryAluno = "UPDATE Alunos SET senha = @senha WHERE Id = @id";
            string queryProfessor = "UPDATE Professores SET senha = @senha WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd;
                    if (entity.Tipo == "Aluno") cmd = new SqlCommand(queryAluno, connection);
                    else cmd = new SqlCommand(queryProfessor, connection);

                    cmd.Parameters.AddWithValue("@senha", entity.Senha);
                    cmd.Parameters.AddWithValue("@id", entity.Id);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginRepository.UpdatePassword: {ex.Message}");
                return false;
            }
        }
    }
}
