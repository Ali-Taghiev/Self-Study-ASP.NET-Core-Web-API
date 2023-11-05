using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
            
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingwalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingwalk == null)
            {
                return null;
            }
             dbContext.Walks.Remove(existingwalk);
            dbContext.SaveChanges();
            return existingwalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filteron = null, string? filterquery = null,
            string? sortBy = null, bool isAscending = true,int pageNumber=1 ,int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering 

            if(string.IsNullOrWhiteSpace(filteron)==false && string.IsNullOrWhiteSpace(filterquery)==false)
            {

                if (filteron.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
                    walks = walks.Where(x => x.Name.Contains(filterquery));
                        }
            }

            //Sorting

            // Sorting

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            //Pagination
            var skippedResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skippedResults).Take(pageSize).ToListAsync();
            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
           return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

       

        public async Task<Walk?> UpdateAsync(Guid id,Walk walk)
        {
            var existingwalk  = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingwalk == null)
            {
                return null;
            }

            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.WalkImageUrl = walk.WalkImageUrl;
            existingwalk.RegionId = walk.RegionId;
            existingwalk.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();

            return existingwalk; 
        }
    }
}
