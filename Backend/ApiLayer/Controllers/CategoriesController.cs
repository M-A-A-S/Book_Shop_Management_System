using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            List<CategoryDTO> CategoriesList = clsCategory.GetAllCategories();
            if (CategoriesList.Count == 0)
            {
                return NotFound("No Categories Found!");
            }
            return Ok(CategoriesList);
        }

        [HttpGet("{Id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> GetCategoryById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            clsCategory Category = clsCategory.Find(Id);

            if (Category == null)
            {
                return NotFound($"Category with ID {Id} not found");
            }

            CategoryDTO CategoryDTO = Category.CategoryDTO;

            return Ok(CategoryDTO);
        }

        [HttpPost(Name = "AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CategoryDTO> AddCategory(CategoryDTO newCategoryDTO)
        {
            if (newCategoryDTO == null || string.IsNullOrEmpty(newCategoryDTO.Name) || string.IsNullOrEmpty(newCategoryDTO.Description))
            {
                return BadRequest("Invalid category data.");
            }
            clsCategory Category = new clsCategory(new CategoryDTO(newCategoryDTO.Id, newCategoryDTO.Name, newCategoryDTO.Description));
            if (!Category.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            newCategoryDTO.Id = Category.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetCategoryById", new { id = newCategoryDTO.Id }, newCategoryDTO);
        }

        [HttpPut("{Id}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CategoryDTO> UpdateCategory(int Id, CategoryDTO updatedCategoryDTO)
        {
            if (Id < 1 || updatedCategoryDTO == null || string.IsNullOrEmpty(updatedCategoryDTO.Name) || string.IsNullOrEmpty(updatedCategoryDTO.Description))
            {
                return BadRequest("Invalid category data.");
            }
            clsCategory Category = clsCategory.Find(Id);
            if (Category == null)
            {
                return NotFound($"Category with ID {Id} not found.");
            }
            Category.Name = updatedCategoryDTO.Name;
            Category.Description = updatedCategoryDTO.Description;
            if (!Category.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            return Ok(Category.CategoryDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategory(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsCategory.DeleteCategory(Id))
            {
                return Ok($"Category with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Category with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
