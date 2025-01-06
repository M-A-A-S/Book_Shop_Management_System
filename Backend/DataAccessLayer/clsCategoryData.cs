using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDTO(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
    }
    public class clsCategoryData
    {
        public static List<CategoryDTO> GetAllCategories()
        {
            var CategoriesList = new List<CategoryDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllCategories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoriesList.Add(new CategoryDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Description"))
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

            return CategoriesList;
        }

        public static CategoryDTO GetCategoryById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetCategoryById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CategoryDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Description"))
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

        public static int AddCategory(CategoryDTO categoryDTO)
        {
            int categoryId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", categoryDTO.Name);
                        command.Parameters.AddWithValue("@Description", categoryDTO.Description);
                        var outputIdParam = new SqlParameter("@NewCategoryId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        categoryId = (int)outputIdParam.Value;
                        return categoryId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return categoryId;
        }

        public static bool UpdateCategory(CategoryDTO categoryDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", categoryDTO.Id);
                        command.Parameters.AddWithValue("@Name", categoryDTO.Name);
                        command.Parameters.AddWithValue("@Description", categoryDTO.Description);
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

        public static bool DeleteCategory(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteCategory", connection))
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
