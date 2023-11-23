using Models;
using Repository.IRepository;
using System.Data.SqlClient;

namespace Repository
{
    public class ProfessorRepository : IUsuarioRepository<ProfessorModel>
    {
        private readonly string cs = string.Empty;

        public ProfessorRepository(string connectionString)
        {
            cs = connectionString;
        }

        /// <summary>
        /// Cria um novo professor no sistema.
        /// </summary>
        /// <param name="entity">Detalhes do professor a ser criado.</param>
        /// <returns>Indicação de sucesso ou falha.</returns>
        public bool Create(ProfessorModel entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Professores (nome, login, senha, email, dataDeCadastro) VALUES (@nome, @login, @senha, @email, @dataDeCadastro)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@login", entity.Login);
                    cmd.Parameters.AddWithValue("@senha", entity.Senha);
                    cmd.Parameters.AddWithValue("@email", entity.Email);
                    cmd.Parameters.AddWithValue("@dataDeCadastro", DateTime.Now);

                    cmd.Connection.Open();

                    return cmd.ExecuteNonQuery() > 0;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ProfessorRepository.Create: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui um professor com base no ID, incluindo suas disciplinas associadas.
        /// </summary>
        /// <param name="id">O ID do professor a ser excluído.</param>
        /// <returns>Indicação de sucesso ou falha.</returns>
        public bool Delete(int id)
        {
            string query = "DELETE FROM Professores WHERE Id = @id";
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
                Console.WriteLine($"Error in ProfessorRepository.Delete: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Obtém todos os professores no sistema.
        /// </summary>
        /// <returns>Lista de professores.</returns>
        public List<ProfessorModel> GetAll()
        {
            List<ProfessorModel> professores = new List<ProfessorModel>();
            string query = "SELECT Id, nome, login, senha, email, dataDeCadastro, dataDeAtualizacao FROM Professores";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ProfessorModel professor = new ProfessorModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["nome"].ToString()!,
                            Login = reader["login"].ToString()!,
                            Senha = reader["senha"].ToString()!,
                            Email = reader["email"].ToString()!,
                            DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") professor.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        professores.Add(professor);
                    }

                }
                return professores;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ProfessorRepositry.GetAll: {ex.Message}");
            }
            return professores;
        }


        /// <summary>
        /// Obtém um professor com base no ID.
        /// </summary>
        /// <param name="id">O ID do professor a ser obtido.</param>
        /// <returns>Detalhes do professor.</returns>
        public ProfessorModel GetById(int id)
        {
            ProfessorModel professor = new ProfessorModel();
            string query = "SELECT Id, nome, login, senha, email, dataDeCadastro, dataDeAtualizacao FROM Professores WHERE Id = @id";
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
                Console.WriteLine($"Error in ProfessorRepositry.GetById: {ex.Message}");
            }
            return professor;
        }


        /// <summary>
        /// Atualiza os detalhes de um professor existente.
        /// </summary>
        /// <param name="entity">Os novos detalhes do professor.</param>
        public void Update(ProfessorModel entity)
        {
            string query = "UPDATE Professores SET nome = @nome, login = @login, email = @email, dataDeAtualizacao = @dataDeAtualizacao WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@login", entity.Login);
                    cmd.Parameters.AddWithValue("@email", entity.Email);
                    cmd.Parameters.AddWithValue("@dataDeAtualizacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", entity.Id);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ProfessorRepository.Update: {ex.Message}");
            }
        }
    }
}
