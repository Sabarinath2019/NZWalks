using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            var walkdomain = await walkRepository.GetAllAsync();

            var walkdto = mapper.Map<List<Models.DTO.Walk>>(walkdomain);
            return Ok(walkdto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get walk domain object  from database
            var walkdomain = await walkRepository.GetAsync(id);

            //convert domain object to DTO
            var walkdto = mapper.Map<Models.DTO.Walk>(walkdomain);

            //return reponse
            return Ok(walkdto);
        }

        [HttpPost]
        public async Task<IActionResult> Addwalkasync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //convert dto to domain object
            var walkdomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                name = addWalkRequest.Name,
                RegionId = addWalkRequest.Regionid,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            walkdomain = await walkRepository.AddAsync(walkdomain);

            var walkDto = new Models.DTO.Walk
            {
                id = walkdomain.Id,
                Length = walkdomain.Length,
                name = walkdomain.name,
                RegionId = walkdomain.RegionId,
                WalkDifficultyId = walkdomain.WalkDifficultyId
            };
            //Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDto.id }, walkDto);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert dto to domain object
            var walkdomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.Regionid,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            walkdomain = await walkRepository.UpdateAsync(id, walkdomain);

            if (walkdomain == null)
            {
                return NotFound();
            }

            var walkdto = new Models.DTO.Walk
            {
                id = walkdomain.Id,
                Length = walkdomain.Length,
                name = walkdomain.name,
                RegionId = walkdomain.RegionId,
                WalkDifficultyId = walkdomain.WalkDifficultyId
            };
            return Ok(walkdto);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkdomain = await walkRepository.DeleteAsync(id);
            if(walkdomain == null)
            {
                return NotFound();
            }
            var walkdto = mapper.Map<Models.DTO.Walk>(walkdomain);

            return Ok(walkdto);
        }
    }
}
