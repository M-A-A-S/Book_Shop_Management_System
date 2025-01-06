using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsCountry
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public CountryDTO CountryDTO
        {
            get
            {
                return new CountryDTO(this.CountryID, this.CountryName);
            }
        }

        public clsCountry(CountryDTO CountryDTO, enMode Mode = enMode.AddNew)
        {
            this.CountryID = CountryDTO.CountryID;
            this.CountryName = CountryDTO.CountryName;
            this.Mode = Mode;
        }
        public static List<CountryDTO> GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
    }
}
