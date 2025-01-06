using DataAccessLayer;

namespace DataBusinessLayer
{

    public class clsAuthor
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public int CountryId { get; set; }
        public AuthorDTO AuthorDTO
        {
            get
            {
                return new AuthorDTO(this.Id, this.Name, this.IsMale, this.CountryId);
            }
        }

        public clsAuthor(AuthorDTO AuthorDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = AuthorDTO.Id;
            this.Name = AuthorDTO.Name;
            this.IsMale = AuthorDTO.IsMale;
            this.CountryId = AuthorDTO.CountryId;
            this.Mode = Mode;
        }

        public bool _AddNewAuthor()
        {
            this.Id = clsAuthorData.AddAuthor(AuthorDTO);
            return (this.Id != -1);
        }
        public bool _UpdateAuthor()
        {
            return clsAuthorData.UpdateAuthor(AuthorDTO);
        }
        public static List<AuthorDTO> GetAllAuthors()
        {
            return clsAuthorData.GetAllAuthors();
        }
        public static clsAuthor Find(int Id)
        {
            AuthorDTO AuthorDTO = clsAuthorData.GetAuthorById(Id);
            if (AuthorDTO != null)
            {
                return new clsAuthor(AuthorDTO, enMode.Update);
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
                    if (_AddNewAuthor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateAuthor();
            }

            return false;
        }
        public static bool DeleteAuthor(int Id)
        {
            return clsAuthorData.DeleteAuthor(Id);
        }
    }
}
