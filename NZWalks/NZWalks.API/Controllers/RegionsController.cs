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

            return CreatedAtAction(nameof(GetRegionsAsync), new {id = regionDto.Id },regionDto);
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
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateregionrequest)
        {
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
            region = await regionRepository.UpdateAsync(id,region);
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
    }
}
