using Microsoft.EntityFrameworkCore;
using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public class VideoRepository : EfRepository<Video>, IVideoRepository
    {
        public VideoRepository(UserContext context) : base(context) { }

        // Отримання схвалених відео для головної сторінки/каталогу
        public async Task<IEnumerable<Video>> GetApprovedVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Genres)
                .Include(v => v.User)
                .Where(v => v.IsApproved && v.IsPublished)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        // Отримання відео, що очікують модерації
        public async Task<IEnumerable<Video>> GetPendingVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Genres)
                .Include(v => v.User)
                .Where(v => !v.IsApproved)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        // Отримання всіх відео з деталями
        public async Task<IEnumerable<Video>> GetAllWithDetailsAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Genres)
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        // Отримання конкретного відео за ID з усіма зв'язками (для Watch.cshtml)
        public async Task<Video?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Videos
                .Include(v => v.User)
                .Include(v => v.Genres)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}