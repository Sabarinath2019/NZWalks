using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        public readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid Id)
        {
            var existingwalk = await nZWalksDbContext.Walks.FindAsync(Id);
            if (existingwalk == null)
            {
                return null;
            }
            nZWalksDbContext.Walks.Remove  (existingwalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid Id, Walk walk)
        {
            var existingwalk = await nZWalksDbContext.Walks.FindAsync(Id);

            if (existingwalk != null)
            {
                existingwalk.Length = walk.Length;
                existingwalk.name = walk.name;
                existingwalk.WalkDifficultyId = walk.WalkDifficultyId;

                existingwalk.RegionId = walk.RegionId;

                await nZWalksDbContext.SaveChangesAsync();
                return existingwalk;
            }
            return null;
        }
    }
}
