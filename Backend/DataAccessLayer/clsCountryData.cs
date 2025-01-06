using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CountryDTO
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public CountryDTO(int id, string countryName)
        {
            this.CountryID = id;
            this.CountryName = countryName;
        }
    }
    public class clsCountryData
    {
        public static List<CountryDTO> GetAllCountries()
        {
            var CountriesList = new List<CountryDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllCountries", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CountriesList.Add(new CountryDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("CountryID")),
                                    reader.GetString(reader.GetOrdinal("CountryName"))
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

            return CountriesList;
        }

    }
}
