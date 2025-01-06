using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsCategory
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryDTO CategoryDTO
        {
            get
            {
                return new CategoryDTO(this.Id, this.Name, this.Description);
            }
        }

        public clsCategory(CategoryDTO CategoryDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = CategoryDTO.Id;
            this.Name = CategoryDTO.Name;
            this.Description = CategoryDTO.Description;
            this.Mode = Mode;
        }

        public bool _AddNewCategory()
        {
            this.Id = clsCategoryData.AddCategory(CategoryDTO);
            return (this.Id != -1);
        }
        public bool _UpdateCategory()
        {
            return clsCategoryData.UpdateCategory(CategoryDTO);
        }
        public static List<CategoryDTO> GetAllCategories()
        {
            return clsCategoryData.GetAllCategories();
        }
        public static clsCategory Find(int Id)
        {
            CategoryDTO CategoryDTO = clsCategoryData.GetCategoryById(Id);
            if (CategoryDTO != null)
            {
                return new clsCategory(CategoryDTO, enMode.Update);
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
                    if (_AddNewCategory())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateCategory();
            }

            return false;
        }
        public static bool DeleteCategory(int Id)
        {
            return clsCategoryData.DeleteCategory(Id);
        }
    }
}
