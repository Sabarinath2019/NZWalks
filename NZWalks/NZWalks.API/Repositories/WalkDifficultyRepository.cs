using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        public readonly NZWalksDbContext nZWalksDbContext;
        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalksDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {

            var existingwalkdifficulty = await nZWalksDbContext.WalksDifficulty.FindAsync(id);
            if (existingwalkdifficulty != null)
            {
                nZWalksDbContext.WalksDifficulty.Remove(existingwalkdifficulty);
                await nZWalksDbContext.SaveChangesAsync();
                return existingwalkdifficulty;
            }
            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalksDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalksDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var exisitingwalkdifficulty = await nZWalksDbContext.WalksDifficulty.FindAsync(id);

            if (exisitingwalkdifficulty == null)
            {
                return null;
            }

            exisitingwalkdifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
           return exisitingwalkdifficulty;
        }
    }
}
