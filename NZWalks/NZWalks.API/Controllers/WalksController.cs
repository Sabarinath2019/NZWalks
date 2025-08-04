using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Threading.Tasks;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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

            //Validate the incoming request
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            //Validate the incoming request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
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
            if (walkdomain == null)
            {
                return NotFound();
            }
            var walkdto = mapper.Map<Models.DTO.Walk>(walkdomain);

            return Ok(walkdto);
        }

        #region Private methods       

        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest)
        {

            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $" Add region data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required ");
            }
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} Should be greater than zero");
            }

            var region = regionRepository.GetAsync(addWalkRequest.Regionid);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Regionid), $"{nameof(addWalkRequest.Regionid)} is Invalid.");
            }
            var walkdifficluty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkdifficluty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                     $"{nameof(addWalkRequest.WalkDifficultyId)} is Invalid.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $" Add region data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required ");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} Should be greater than zero");
            }

            var region = regionRepository.GetAsync(updateWalkRequest.Regionid);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Regionid), $"{nameof(updateWalkRequest.Regionid)} is Invalid.");
            }
            var walkdifficluty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkdifficluty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                     $"{nameof(updateWalkRequest.WalkDifficultyId)} is Invalid.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
    }
    #endregion
}
