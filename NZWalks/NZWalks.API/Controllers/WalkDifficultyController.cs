using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Drawing.Drawing2D;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        public readonly IWalkDifficultyRepository walkDifficultyRepository;
        public readonly IMapper mapper;
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllwalkDifficulties()
        {

            var walkdifficultydomain = await walkDifficultyRepository.GetAllAsync();
            var walkdifficultyDto = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkdifficultydomain);
            return Ok(walkdifficultyDto);

            // return Ok(await walkDifficultyRepository.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAllwalkDifficultiesbyId")]
        public async Task<IActionResult> GetAllwalkDifficultiesbyId(Guid id)
        {
            var walkdifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkdifficulty == null)
            {
                return NotFound();
            }

            var walkdifficultyDto = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficulty);
            return Ok(walkdifficultyDto);
        }

        [HttpPost]
        public async Task<IActionResult> Addwalkdifficultyasync(
            AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            var walkdifficultydomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            walkdifficultydomain = await walkDifficultyRepository.AddAsync(walkdifficultydomain);
            var WalkdifficultyDto = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);
            return CreatedAtAction(nameof(GetAllwalkDifficultiesbyId), new { id = WalkdifficultyDto.Id }, WalkdifficultyDto);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Updatewalkdifficultyasync(Guid id,
            UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            var walkdifficultydomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };
            walkdifficultydomain = await walkDifficultyRepository.UpdateAsync(id, walkdifficultydomain);

            if (walkdifficultydomain == null)
            {
                return NotFound();
            }
            var WalkdifficultyDto = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);
            return Ok(WalkdifficultyDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
       
        public async Task<IActionResult> DeletewalkDifficulty(Guid id)
        {
            var walkdifficultydomain =await walkDifficultyRepository.DeleteAsync(id);

            if(walkdifficultydomain == null)
            {
                return NotFound();
            }
            var WalkdifficultyDto = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);
            return Ok(WalkdifficultyDto);
        }
    }
}
