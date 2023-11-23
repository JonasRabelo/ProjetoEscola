using Models;
using Models.Enums;
using Repository.IRepository;
using System.Data.SqlClient;

namespace Repository
{
    public class SARepository : ISARepository<SuperUserModel>
    {
        private readonly string cs = "server=DESKTOP-MQADPEC\\SQLEXPRESS; database=DB_EscolaMJV; Trusted_Connection = true; Integrated Security=SSPI;TrustServerCertificate=True";

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
