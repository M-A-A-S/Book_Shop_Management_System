using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsBook
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public BookDTO BookDTO
        {
            get
            {
                return new BookDTO(this.Id, this.Title, this.AuthorId, this.CategoryId, this.Price, this.Quantity);
            }
        }

        public clsBook(BookDTO BookDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = BookDTO.Id;
            this.Title = BookDTO.Title;
            this.AuthorId = BookDTO.AuthorId;
            this.CategoryId = BookDTO.CategoryId;
            this.Price = BookDTO.Price;
            this.Quantity = BookDTO.Quantity;
            this.Mode = Mode;
        }

        public bool _AddNewBook()
        {
            this.Id = clsBookData.AddBook(BookDTO);
            return (this.Id != -1);
        }
        public bool _UpdateBook()
        {
            return clsBookData.UpdateBook(BookDTO);
        }
        public static List<BookDTO> GetAllBooks()
        {
            return clsBookData.GetAllBooks();
        }
        public static clsBook Find(int Id)
        {
            BookDTO BookDTO = clsBookData.GetBookById(Id);
            if (BookDTO != null)
            {
                return new clsBook(BookDTO, enMode.Update);
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
                    if (_AddNewBook())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateBook();
            }

            return false;
        }
        public static bool DeleteBook(int Id)
        {
            return clsBookData.DeleteBook(Id);
        }
    }
}
