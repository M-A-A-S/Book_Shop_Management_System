using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Sellers")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllSellers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SellerDTO>> GetAllSellers()
        {
            List<SellerDTO> SellersList = clsSeller.GetAllSellers();
            if (SellersList.Count == 0)
            {
                return NotFound("No Sellers Found!");
            }
            return Ok(SellersList);
        }

        [HttpGet("{Id}", Name = "GetSellerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SellerDTO> GetSellerById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            clsSeller Seller = clsSeller.Find(Id);
            if (Seller == null)
            {
                return NotFound($"Seller with ID {Id} not found");
            }
            SellerDTO SellerDTO = Seller.SellerDTO;
            return Ok(SellerDTO);
        }

        [HttpPost(Name = "AddSeller")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SellerDTO> AddSeller(SellerDTO newSellerDTO)
        {
            if (newSellerDTO == null || 
                string.IsNullOrEmpty(newSellerDTO.Name) || 
                string.IsNullOrEmpty(newSellerDTO.Email) || 
                string.IsNullOrEmpty(newSellerDTO.Phone) || 
                string.IsNullOrEmpty(newSellerDTO.Address)
                )
            {
                return BadRequest("Invalid seller data.");
            }
            clsSeller Seller = new clsSeller(new SellerDTO(newSellerDTO.Id, newSellerDTO.Name, newSellerDTO.Email, newSellerDTO.Phone, newSellerDTO.Address));
            if (!Seller.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            newSellerDTO.Id = Seller.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetSellerById", new { id = newSellerDTO.Id }, newSellerDTO);
        }

        [HttpPut("{Id}", Name = "UpdateSeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SellerDTO> UpdateSeller(int Id, SellerDTO updatedSellerDTO)
        {
            if (Id < 1 || updatedSellerDTO == null ||
                string.IsNullOrEmpty(updatedSellerDTO.Name) ||
                string.IsNullOrEmpty(updatedSellerDTO.Email) ||
                string.IsNullOrEmpty(updatedSellerDTO.Phone) ||
                string.IsNullOrEmpty(updatedSellerDTO.Address)
                )
            {
                return BadRequest("Invalid seller data.");
            }
            clsSeller Seller = clsSeller.Find(Id);
            if (Seller == null)
            {
                return NotFound($"Seller with ID {Id} not found.");
            }
            Seller.Name = updatedSellerDTO.Name;
            Seller.Email = updatedSellerDTO.Email;
            Seller.Phone = updatedSellerDTO.Phone;
            Seller.Address = updatedSellerDTO.Address;
            if (!Seller.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            return Ok(Seller.SellerDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteSeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteSeller(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }

            if (clsSeller.DeleteSeller(Id))
            {
                return Ok($"Seller with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Seller with ID {Id} not found. no rows deleted!");
            }
        }
    }
}
