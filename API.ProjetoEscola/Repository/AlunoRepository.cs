using Models;
using Models.Enums;
using Repository.IRepository;
using System.Data.SqlClient;

namespace Repository
{
    public class AlunoRepository : IUsuarioRepository<AlunoModel>
    {
        private readonly string cs = string.Empty;

        public AlunoRepository(string connectionString) {
            this.cs = connectionString;
        }


        /// <summary>
        /// Cria um novo aluno no sistema.
        /// </summary>
        /// <param name="entity">Detalhes do aluno a ser criado.</param>
        /// <returns>Indicação de sucesso ou falha.</returns>
        public bool Create(AlunoModel entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Alunos (nome, login, senha, email, serie, dataDeCadastro) VALUES (@nome, @login, @senha, @email, @serie, @dataDeCadastro)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@login", entity.Login);
                    cmd.Parameters.AddWithValue("@senha", entity.Senha);
                    cmd.Parameters.AddWithValue("@email", entity.Email);
                    cmd.Parameters.AddWithValue("@serie", entity.Serie);
                    cmd.Parameters.AddWithValue("@dataDeCadastro", DateTime.Now);


                    cmd.Connection.Open();

                    return cmd.ExecuteNonQuery() > 0;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AlunoRepository.Create: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui um aluno com base no ID.
        /// </summary>
        /// <param name="id">O ID do aluno a ser excluído.</param>
        /// <returns>Indicação de sucesso ou falha.</returns>
        public bool Delete(int id)
        {
            string query = "DELETE FROM Alunos WHERE Id = @id";
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
                Console.WriteLine($"Error in AlunoRepository.Delete: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Obtém todos os alunos no sistema.
        /// </summary>
        /// <returns>Lista de alunos.</returns>
        public List<AlunoModel> GetAll()
        {
            List<AlunoModel> alunos = new List<AlunoModel>();
            string query = "SELECT Id, nome, login, senha, email, serie, dataDeCadastro, dataDeAtualizacao FROM Alunos";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        AlunoModel aluno = new AlunoModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["nome"].ToString()!,
                            Login = reader["login"].ToString()!,
                            Senha = reader["senha"].ToString()!,
                            Email = reader["email"].ToString()!,
                            Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!),
                            DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") aluno.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        alunos.Add(aluno);
                    }

                }
                return alunos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AlunoRepositry.GetAll: {ex.Message}");
            }
            return alunos;
        }


        /// <summary>
        /// Obtém um aluno com base no ID.
        /// </summary>
        /// <param name="id">O ID do aluno a ser obtido.</param>
        /// <returns>Detalhes do aluno.</returns>
        public AlunoModel GetById(int id)
        {
            AlunoModel aluno = new AlunoModel();
            string query = "SELECT Id, nome, login, senha, email, serie, dataDeCadastro, dataDeAtualizacao FROM Alunos WHERE Id = @id";
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
                Console.WriteLine($"Error in AlunoRepositry.GetById: {ex.Message}");
            }
            return aluno;
        }


        /// <summary>
        /// Atualiza os detalhes de um aluno existente.
        /// </summary>
        /// <param name="entity">Os novos detalhes do aluno.</param>
        public void Update(AlunoModel entity)
        {
            string query = "UPDATE Alunos SET nome = @nome, login = @login, email = @email, serie = @serie, dataDeAtualizacao = @dataDeAtualizacao WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@login", entity.Login);
                    cmd.Parameters.AddWithValue("@email", entity.Email);
                    cmd.Parameters.AddWithValue("@serie", entity.Serie);
                    cmd.Parameters.AddWithValue("@dataDeAtualizacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", entity.Id);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AlunoRepository.Update: {ex.Message}");
            }
        }
    }
}
