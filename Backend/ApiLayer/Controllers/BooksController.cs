using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooks()
        {
            List<BookDTO> BooksList = clsBook.GetAllBooks();
            if (BooksList.Count == 0)
            {
                return NotFound("No Books Found!");
            }
            return Ok(BooksList);
        }

        [HttpGet("{Id}", Name = "GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookDTO> GetBookById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsBook Book = clsBook.Find(Id);

            if (Book == null)
            {
                return NotFound($"Book with ID {Id} not found");
            }

            BookDTO BookDTO = Book.BookDTO;

            return Ok(BookDTO);
        }

        [HttpPost(Name = "AddBook")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BookDTO> AddBook(BookDTO newBookDTO)
        {
            if (newBookDTO == null || string.IsNullOrEmpty(newBookDTO.Title) || newBookDTO.AuthorId < 1 || newBookDTO.CategoryId < 1)
            {
                return BadRequest("Invalid book data.");
            }

            clsBook Book = new clsBook(new BookDTO(newBookDTO.Id, newBookDTO.Title, newBookDTO.AuthorId, newBookDTO.CategoryId, newBookDTO.Price, newBookDTO.Quantity));
            if (!Book.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            newBookDTO.Id = Book.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetBookById", new { id = newBookDTO.Id }, newBookDTO);
        }

        [HttpPut("{Id}", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BookDTO> UpdateBook(int Id, BookDTO updatedBookDTO)
        {
            if (Id < 1 || updatedBookDTO == null || string.IsNullOrEmpty(updatedBookDTO.Title) || updatedBookDTO.AuthorId < 1 || updatedBookDTO.CategoryId < 1)
            {
                return BadRequest("Invalid book data.");
            }

            clsBook Book = clsBook.Find(Id);

            if (Book == null)
            {
                return NotFound($"Book with ID {Id} not found.");
            }

            Book.Title = updatedBookDTO.Title;
            Book.AuthorId = updatedBookDTO.AuthorId;
            Book.CategoryId = updatedBookDTO.CategoryId;
            Book.Price = updatedBookDTO.Price;
            Book.Quantity = updatedBookDTO.Quantity;
            if (!Book.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return Ok(Book.BookDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBook(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsBook.DeleteBook(Id))
            {
                return Ok($"Book with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Book with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
