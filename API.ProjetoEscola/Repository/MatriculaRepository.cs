using Models.Enums;
using Models;
using System.Data.SqlClient;
using Repository.IRepository;

namespace Repository
{
    public class MatriculaRepository : IMatriculaRepository<MatriculaModel>
    {
        private readonly string cs = "server=DESKTOP-MQADPEC\\SQLEXPRESS; database=DB_EscolaMJV; Trusted_Connection = true; Integrated Security=SSPI;TrustServerCertificate=True";
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


        public List<MatriculaModel> GetAllByIdDiscipline(int id)
        {
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas WHERE disciplinaId = @id";
            return GetAll(query, id);
        }


        public List<MatriculaModel> GetAllByIdStudent(int id)
        {
            string query = "SELECT Id, nota1, nota2, nota3, nota4, mediaFinal, alunoId, disciplinaId, dataMatricula, dataDeAtualizacao FROM Matriculas WHERE alunoId = @id";
            return GetAll(query, id);
        }


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
