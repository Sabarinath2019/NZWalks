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

            //validate the request
            if (!await ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

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
            //validate the request
            if (!await ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

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
            var walkdifficultydomain = await walkDifficultyRepository.DeleteAsync(id);

            if (walkdifficultydomain == null)
            {
                return NotFound();
            }
            var WalkdifficultyDto = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);
            return Ok(WalkdifficultyDto);
        }

        #region Private methods       

        private async Task<bool> ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkdifficultyRequest)
        {
            if(addWalkdifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkdifficultyRequest), $" addWalkdifficultyRequest data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkdifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkdifficultyRequest.Code), $"{nameof(addWalkdifficultyRequest.Code)} Cannot be null or Empty or white space ");
            }
            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkDifficultyAsync(UpdateWalkDifficultyRequest updateWalkdifficultyRequest)
        {
            if (updateWalkdifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkdifficultyRequest), $" addWalkdifficultyRequest data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkdifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkdifficultyRequest.Code), $"{nameof(updateWalkdifficultyRequest.Code)} Cannot be null or Empty or white space ");
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
