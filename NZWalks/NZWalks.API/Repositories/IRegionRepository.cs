using NZWalks.API.Models;
using NZWalks.API.Models.Domain;
using Region = NZWalks.API.Models.Domain.Region;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
       Task<IEnumerable<Region>> GetAllAsync();
    }
}
