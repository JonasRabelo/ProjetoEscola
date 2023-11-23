using Models;
using Models.Enums;
using Repository.IRepository;
using System.Data.SqlClient;

namespace Repository
{
    /// <summary>
    /// Fornece funcionalidades de acesso para buscar o registrodo superusuário no banco de dados.
    /// </summary>
    public class SARepository : ISARepository<SuperUserModel>
    {
        private readonly string cs = string.Empty;

        public SARepository(string connectionString)
        {
            cs = connectionString;
        }

        /// <summary>
        /// Verifica se um superusuário com as credenciais fornecidas existe no banco de dados.
        /// </summary>
        /// <param name="login">O login do superusuário.</param>
        /// <param name="senha">A senha do superusuário.</param>
        /// <returns>True se um superusuário com as credenciais fornecidas existir, False caso contrário.</returns>
        public bool Get(string login, string senha)
        {
            string query = "SELECT * FROM SuperUser WHERE login = @login AND senha = @senha";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SuperUserRepository.Get: {ex.Message}");
            }
            return false;
        }
    }
}
