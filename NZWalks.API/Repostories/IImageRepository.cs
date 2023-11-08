using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostories
{
    public interface IImageRepository
    {

        Task<Image> Upload(Image image);
    }
}
