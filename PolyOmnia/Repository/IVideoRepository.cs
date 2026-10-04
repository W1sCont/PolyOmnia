using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> GetApprovedVideosAsync();
        Task<IEnumerable<Video>> GetPendingVideosAsync();
        Task<IEnumerable<Video>> GetAllWithDetailsAsync();
        Task<Video?> GetByIdWithDetailsAsync(int id);
    }
}
