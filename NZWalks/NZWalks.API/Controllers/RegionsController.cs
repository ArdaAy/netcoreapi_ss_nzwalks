using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Region 1",
                    Code = "Code 1",
                    RegionImageUrl = "SampleUrl 1"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Region 2",
                    Code = "Code 2"
                }
            };

            return Ok(regions);
        }
    }
}
