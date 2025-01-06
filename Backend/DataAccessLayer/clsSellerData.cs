using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class SellerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public SellerDTO(int id, string name, string email, string phone, string address)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
        }
    }
    public class clsSellerData
    {
        public static List<SellerDTO> GetAllSellers()
        {
            var SellersList = new List<SellerDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllSellers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SellersList.Add(new SellerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Email")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetString(reader.GetOrdinal("Address"))
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

            return SellersList;
        }

        public static SellerDTO GetSellerById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetSellerById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SellerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Email")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetString(reader.GetOrdinal("Address"))
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

        public static int AddSeller(SellerDTO sellerDTO)
        {
            int sellerId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewSeller", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", sellerDTO.Name);
                        command.Parameters.AddWithValue("@Email", sellerDTO.Email);
                        command.Parameters.AddWithValue("@Phone", sellerDTO.Phone);
                        command.Parameters.AddWithValue("@Address", sellerDTO.Address);
                        var outputIdParam = new SqlParameter("@NewSellerId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        sellerId = (int)outputIdParam.Value;
                        return sellerId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return sellerId;
        }

        public static bool UpdateSeller(SellerDTO sellerDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateSeller", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", sellerDTO.Id);
                        command.Parameters.AddWithValue("@Name", sellerDTO.Name);
                        command.Parameters.AddWithValue("@Email", sellerDTO.Email);
                        command.Parameters.AddWithValue("@Phone", sellerDTO.Phone);
                        command.Parameters.AddWithValue("@Address", sellerDTO.Address);
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

        public static bool DeleteSeller(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteSeller", connection))
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
