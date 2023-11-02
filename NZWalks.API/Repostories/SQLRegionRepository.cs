using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
           var existingregion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingregion == null)
            {
                return null;

            }

            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.RegionImageUrl= region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return existingregion;
        }

        public async Task<List<Region>> GetAllasync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x =>x.Id==id);
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var DeletedRegion = await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id==id);
            if (DeletedRegion==null)
            {
                return null;
            }

            dbContext.Regions.Remove(DeletedRegion);
            await dbContext.SaveChangesAsync();

            return DeletedRegion;
        }
    }

}
