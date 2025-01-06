using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsSeller
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public SellerDTO SellerDTO
        {
            get
            {
                return new SellerDTO(this.Id, this.Name, this.Email, this.Phone, this.Address);
            }
        }

        public clsSeller(SellerDTO SellerDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = SellerDTO.Id;
            this.Name = SellerDTO.Name;
            this.Email = SellerDTO.Email;
            this.Phone = SellerDTO.Phone;
            this.Address = SellerDTO.Address;
            this.Mode = Mode;
        }

        public bool _AddNewSeller()
        {
            this.Id = clsSellerData.AddSeller(SellerDTO);
            return (this.Id != -1);
        }
        public bool _UpdateSeller()
        {
            return clsSellerData.UpdateSeller(SellerDTO);
        }

        public static List<SellerDTO> GetAllSellers()
        {
            return clsSellerData.GetAllSellers();
        }
        public static clsSeller Find(int Id)
        {
            SellerDTO SellerDTO = clsSellerData.GetSellerById(Id);
            if (SellerDTO != null)
            {
                return new clsSeller(SellerDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewSeller())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateSeller();
            }

            return false;
        }
        public static bool DeleteSeller(int Id)
        {
            return clsSellerData.DeleteSeller(Id);
        }
    }
}
