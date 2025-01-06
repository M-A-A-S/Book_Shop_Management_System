using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public BookDTO(int id, string title, int authorId, int categoryId, decimal price, int quantity)
        {
            this.Id = id;
            this.Title = title;
            this.AuthorId = authorId;
            this.CategoryId = categoryId;
            this.Price = price;
            this.Quantity = quantity;
        }
    }
    public class clsBookData
    {
        public static List<BookDTO> GetAllBooks()
        {
            var BooksList = new List<BookDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllBooks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BooksList.Add(new BookDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Title")),
                                    reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                    reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetInt32(reader.GetOrdinal("Quantity"))
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

            return BooksList;
        }

        public static BookDTO GetBookById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetBookById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new BookDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Title")),
                                    reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                    reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetInt32(reader.GetOrdinal("Quantity"))
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

        public static int AddBook(BookDTO bookDTO)
        {
            int bookId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewBook", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Title", bookDTO.Title);
                        command.Parameters.AddWithValue("@AuthorId", bookDTO.AuthorId);
                        command.Parameters.AddWithValue("@CategoryId", bookDTO.CategoryId);
                        command.Parameters.AddWithValue("@Price", bookDTO.Price);
                        command.Parameters.AddWithValue("@Quantity", bookDTO.Quantity);
                        var outputIdParam = new SqlParameter("@NewBookId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        bookId = (int)outputIdParam.Value;
                        return bookId;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return bookId;
        }

        public static bool UpdateBook(BookDTO bookDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateBook", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", bookDTO.Id);
                        command.Parameters.AddWithValue("@Title", bookDTO.Title);
                        command.Parameters.AddWithValue("@AuthorId", bookDTO.AuthorId);
                        command.Parameters.AddWithValue("@CategoryId", bookDTO.CategoryId);
                        command.Parameters.AddWithValue("@Price", bookDTO.Price);
                        command.Parameters.AddWithValue("@Quantity", bookDTO.Quantity);
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

        public static bool DeleteBook(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteBook", connection))
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
