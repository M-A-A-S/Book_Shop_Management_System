using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AuthorDTO>> GetAllAuthors()
        {
            List<AuthorDTO> AuthorsList = clsAuthor.GetAllAuthors();
            if (AuthorsList.Count == 0)
            {
                return NotFound("No Authors Found!");
            }
            return Ok(AuthorsList);
        }

        [HttpGet("{Id}", Name = "GetAuthorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuthorDTO> GetAuthorById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsAuthor Author = clsAuthor.Find(Id);

            if (Author == null)
            {
                return NotFound($"Author with ID {Id} not found");
            }

            AuthorDTO AuthorDTO = Author.AuthorDTO;

            return Ok(AuthorDTO);
        }

        [HttpPost(Name = "AddAuthor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AuthorDTO> AddAuthor(AuthorDTO newAuthorDTO)
        {
            if (newAuthorDTO == null || string.IsNullOrEmpty(newAuthorDTO.Name) || newAuthorDTO.CountryId < 1)
            {
                return BadRequest("Invalid author data.");
            }

            clsAuthor Author = new clsAuthor(new AuthorDTO(newAuthorDTO.Id, newAuthorDTO.Name, newAuthorDTO.IsMale, newAuthorDTO.CountryId));
            if (!Author.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            newAuthorDTO.Id = Author.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetAuthorById", new { id = newAuthorDTO.Id }, newAuthorDTO);
        }

        [HttpPut("{Id}", Name = "UpdateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AuthorDTO> UpdateAuthor(int Id, AuthorDTO updatedAuthorDTO)
        {
            if (Id < 1 || updatedAuthorDTO == null || string.IsNullOrEmpty(updatedAuthorDTO.Name) || updatedAuthorDTO.CountryId < 1)
            {
                return BadRequest("Invalid author data.");
            }

            clsAuthor Author = clsAuthor.Find(Id);

            if (Author == null)
            {
                return NotFound($"Author with ID {Id} not found.");
            }

            Author.Name = updatedAuthorDTO.Name;
            Author.IsMale = updatedAuthorDTO.IsMale;
            Author.CountryId = updatedAuthorDTO.CountryId;
            if (!Author.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return Ok(Author.AuthorDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAuthor(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsAuthor.DeleteAuthor(Id))
            {
                return Ok($"Author with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Author with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
