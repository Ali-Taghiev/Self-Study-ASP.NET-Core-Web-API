using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostories
{
    public interface IRegionRepository
    {
       Task<List<Region>> GetAllasync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id,Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
