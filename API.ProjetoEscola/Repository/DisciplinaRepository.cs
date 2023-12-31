﻿using Models;
using Models.Enums;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DisciplinaRepository : IDisciplinaRepository<DisciplinaModel>
    {
        private readonly string cs = string.Empty;

        public DisciplinaRepository(string connectionString)
        {
            cs = connectionString;
        }


        /// <summary>
        /// Cria uma nova disciplina no banco de dados.
        /// </summary>
        /// <param name="entity">A disciplina a ser criada.</param>
        /// <returns>True se a disciplina for criada com sucesso, False caso contrário.</returns>
        public bool Create(DisciplinaModel entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Disciplinas (nome, serie, status, professorId, dataDeCadastro) VALUES (@nome, @serie, @status, @professorId, @dataDeCadastro)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@serie", entity.Serie);
                    cmd.Parameters.AddWithValue("@status", entity.Status);
                    cmd.Parameters.AddWithValue("@professorId", entity.ProfessorId);
                    cmd.Parameters.AddWithValue("@dataDeCadastro", DateTime.Now);

                    cmd.Connection.Open();

                    return cmd.ExecuteNonQuery() > 0;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepository.Create: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Exclui uma disciplina com base no ID.
        /// </summary>
        /// <param name="id">O ID da disciplina a ser excluída.</param>
        /// <returns>True se a disciplina for excluída com sucesso, False caso contrário.</returns>
        public bool Delete(int id)
        {
            string query = "DELETE FROM Disciplinas WHERE Id = @id";

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
                Console.WriteLine($"Error in DisciplinaRepository.Delete: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Obtém todas as disciplinas associadas a um professor com base no ID do professor.
        /// </summary>
        /// <param name="id">O ID do professor.</param>
        /// <returns>Uma lista de disciplinas associadas ao professor.</returns>
        public List<DisciplinaModel> GetAllByIdTeacher(int id)
        {
            List<DisciplinaModel> disciplinas = new List<DisciplinaModel>();
            string query = "SELECT Id, nome, serie, status, professorId, dataDeCadastro, dataDeAtualizacao FROM Disciplinas WHERE professorId = @professorId";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@professorId", id);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DisciplinaModel disciplina = new DisciplinaModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["nome"].ToString()!,
                            Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!),
                            Status = bool.Parse(reader["status"].ToString()!),
                            ProfessorId = Convert.ToInt32(reader["professorId"]),
                            DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") disciplina.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        disciplinas.Add(disciplina);
                    }

                }
                return disciplinas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepositry.GetAllByIdTeacher: {ex.Message}");
            }
            return disciplinas;
        }


        /// <summary>
        /// Obtém todas as disciplinas no banco de dados.
        /// </summary>
        /// <returns>Uma lista de todas as disciplinas.</returns>
        public List<DisciplinaModel> GetAll()
        {
            List<DisciplinaModel> disciplinas = new List<DisciplinaModel>();
            string query = "SELECT Id, nome, serie, status, professorId, dataDeCadastro, dataDeAtualizacao FROM Disciplinas";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DisciplinaModel disciplina = new DisciplinaModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["nome"].ToString()!,
                            Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!),
                            Status = bool.Parse(reader["status"].ToString()!),
                            ProfessorId = Convert.ToInt32(reader["professorId"]),
                            DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") disciplina.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        disciplinas.Add(disciplina);
                    }

                }
                return disciplinas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepositry.GetAll: {ex.Message}");
            }
            return disciplinas;
        }


        /// <summary>
        /// Obtém uma disciplina com base no ID.
        /// </summary>
        /// <param name="id">O ID da disciplina.</param>
        /// <returns>A disciplina encontrada.</returns>
        public DisciplinaModel GetById(int id)
        {
            DisciplinaModel disciplina = new DisciplinaModel();
            string query = "SELECT Id, nome, serie, status, professorId, dataDeCadastro, dataDeAtualizacao FROM Disciplinas WHERE Id = @id";
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
                        disciplina.Id = Convert.ToInt32(reader["Id"]);
                        disciplina.Nome = reader["nome"].ToString()!;
                        disciplina.Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!);
                        disciplina.Status = bool.Parse(reader["status"].ToString()!);
                        disciplina.ProfessorId = Convert.ToInt32(reader["professorId"]);
                        disciplina.DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!);
                        if (reader["dataDeAtualizacao"].ToString()! != "") disciplina.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!); 
                    }
                }
                return disciplina;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepositry.GetById: {ex.Message}");
            }
            return disciplina;
        }


        /// <summary>
        /// Atualiza uma disciplina no banco de dados.
        /// </summary>
        /// <param name="entity">A disciplina a ser atualizada.</param>
        public void Update(DisciplinaModel entity)
        {
            string query = @"UPDATE Disciplinas SET nome = @nome, serie = @serie, status = @status, professorId = @professorId, dataDeAtualizacao = @dataDeAtualizacao WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@nome", entity.Nome);
                    cmd.Parameters.AddWithValue("@serie", entity.Serie);
                    cmd.Parameters.AddWithValue("@status", entity.Status);
                    cmd.Parameters.AddWithValue("@professorId", entity.ProfessorId);
                    cmd.Parameters.AddWithValue("@dataDeAtualizacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", entity.Id);

                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepository.Update: {ex.Message}");
            }
        }


        /// <summary>
        /// Obtém todas as disciplinas de uma determinada série.
        /// </summary>
        /// <param name="serie">A série das disciplinas a serem obtidas.</param>
        /// <returns>Uma lista de disciplinas da série especificada.</returns>
        public List<DisciplinaModel> GetAllBySerie(int serie)
        {
            List<DisciplinaModel> disciplinas = new List<DisciplinaModel>();
            string query = "SELECT Id, nome, serie, status, professorId, dataDeCadastro, dataDeAtualizacao FROM Disciplinas WHERE serie = @serie";
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@serie", serie);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DisciplinaModel disciplina = new DisciplinaModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["nome"].ToString()!,
                            Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!),
                            Status = bool.Parse(reader["status"].ToString()!),
                            ProfessorId = Convert.ToInt32(reader["professorId"]),
                            DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                        };
                        if (reader["dataDeAtualizacao"].ToString()! != "") disciplina.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                        disciplinas.Add(disciplina);
                    }

                }
                return disciplinas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepositry.GetAllByIdTeacher: {ex.Message}");
            }
            return disciplinas;
        }


        /// <summary>
        /// Exclui todas as disciplinas associadas a um professor com base no ID do professor.
        /// </summary>
        /// <param name="teacherId">O ID do professor.</param>
        /// <returns>True se as disciplinas foram excluídas com sucesso, False caso contrário.</returns>
        public bool DeleteByIdTeacher(int teacherId)
        {
            string query = "DELETE FROM Disciplinas WHERE professorId = @teacherId";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@teacherId", teacherId);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepository.DeleteByIdTeacher: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Atualiza o status de uma disciplina no banco de dados.
        /// </summary>
        /// <param name="status">O novo status da disciplina.</param>
        /// <param name="disciplineId">O ID da disciplina a ser atualizada.</param>
        /// <returns>True se a disciplina foi atualizada com sucesso, False caso contrário.</returns>
        public bool UpdateStatus(bool status, int disciplineId)
        {
            string query = @"UPDATE Disciplinas SET status = @status, dataDeAtualizacao = @dataDeAtualizacao WHERE Id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@dataDeAtualizacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", disciplineId);

                    connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisciplinaRepository.Update: {ex.Message}");
                return false;
            }
        }
    }
}
