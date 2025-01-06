using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public int CountryId { get; set; }

        public AuthorDTO(int id, string name, bool isMale, int countryId)
        {
            this.Id = id;
            this.Name = name;
            this.IsMale = isMale;
            this.CountryId = countryId;
        }
    }
    public class clsAuthorData
    {
        public static List<AuthorDTO> GetAllAuthors()
        {
            var AuthorsList = new List<AuthorDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllAuthors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AuthorsList.Add(new AuthorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetBoolean(reader.GetOrdinal("IsMale")),
                                    reader.GetInt32(reader.GetOrdinal("CountryId"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return AuthorsList;
        }

        public static AuthorDTO GetAuthorById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetAuthorById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new AuthorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetBoolean(reader.GetOrdinal("IsMale")),
                                    reader.GetInt32(reader.GetOrdinal("CountryId"))
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return null;
        }

        public static int AddAuthor(AuthorDTO authorDTO)
        {
            int authorId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewAuthor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", authorDTO.Name);
                        command.Parameters.AddWithValue("@IsMale", authorDTO.IsMale);
                        command.Parameters.AddWithValue("@CountryId", authorDTO.CountryId);
                        var outputIdParam = new SqlParameter("@NewAuthorId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        authorId = (int)outputIdParam.Value;
                        return authorId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return authorId;
        }

        public static bool UpdateAuthor(AuthorDTO authorDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateAuthor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", authorDTO.Id);
                        command.Parameters.AddWithValue("@Name", authorDTO.Name);
                        command.Parameters.AddWithValue("@IsMale", authorDTO.IsMale);
                        command.Parameters.AddWithValue("@CountryId", authorDTO.CountryId);
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteAuthor(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteAuthor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        rowsAffected = (int)command.ExecuteNonQuery();
                        return (rowsAffected == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return (rowsAffected > 0);
        }

    }
}
