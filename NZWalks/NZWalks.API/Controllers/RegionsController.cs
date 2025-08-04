using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();

            //var regionsdto = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(region =>
            //{
            //    var regiondto = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population

            //    };
            //    regionsdto.Add(regiondto);

            //});

            var regionDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regions);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionsAsync")]
        public async Task<IActionResult> GetRegionsAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //validate the request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }


            // Request (DTO ) to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };
            region = await regionRepository.AddAsync(region);

            var regionDto = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionsAsync), new { id = regionDto.Id }, regionDto);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from databse
            var region = await regionRepository.DeleteAsync(id);
            //If null notfound
            if (region == null)
            {
                return NotFound();
            }
            //Conver response back to Dto
            var regiondto = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            //return Ok response
            return Ok(regiondto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateregionrequest)
        {

            //validate the request
            if (!ValidateupdateRegionAsync(updateregionrequest))
            {
                return BadRequest(ModelState);
            }
            //convert dto to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateregionrequest.Code,
                Name = updateregionrequest.Name,
                Area = updateregionrequest.Area,
                Lat = updateregionrequest.Lat,
                Long = updateregionrequest.Long,
                Population = updateregionrequest.Population
            };
            //update region to repository
            region = await regionRepository.UpdateAsync(id, region);
            //if Null then Notfound

            if (region == null)
            {
                NotFound();
            }
            //convert domain back to dto
            var regiondto = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            //return Ok response
            return Ok(regiondto);
        }
        #region Private methods       

        private bool ValidateAddRegionAsync(AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $" Add region data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} Cannot be null or Empty or white space ");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} Cannot be null or Empty or white space ");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} Cannot be less than or equal to zero. ");
            }
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} Cannot be less than or equal to zero. ");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} Cannot be less than or equal to zero. ");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} Cannot be less than zero. ");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateupdateRegionAsync(UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $" Add region data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} Cannot be null or Empty or white space ");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} Cannot be null or Empty or white space ");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} Cannot be less than or equal to zero. ");
            }
            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat), $"{nameof(updateRegionRequest.Lat)} Cannot be less than or equal to zero. ");
            }
            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long), $"{nameof(updateRegionRequest.Long)} Cannot be less than or equal to zero. ");
            }
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} Cannot be less than zero. ");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
