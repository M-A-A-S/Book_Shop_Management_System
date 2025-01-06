using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CountryDTO>> GetAllCountries()
        {
            List<CountryDTO> CountriesList = clsCountry.GetAllCountries();
            if (CountriesList.Count == 0)
            {
                return NotFound("No Countries Found!");
            }
            return Ok(CountriesList);
        }
    }
}
