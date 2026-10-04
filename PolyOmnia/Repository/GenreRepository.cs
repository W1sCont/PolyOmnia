using Microsoft.EntityFrameworkCore;
using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public class GenreRepository : EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(UserContext context) : base(context) { }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Genres
                .AnyAsync(g => g.Name == name);
        }

        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds)
        {
            return await _context.Genres
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync();
        }

        public async Task<Genre?> GetGenreWithVideosAsync(int genreId)
        {
            return await _context.Genres
                .Include(g => g.Videos)
                .FirstOrDefaultAsync(g => g.Id == genreId);
        }

        public async Task<IEnumerable<Genre>> GetAllWithVideoCountAsync()
        {
            return await _context.Genres
                .Include(g => g.Videos)
                .ToListAsync();
        }
    }
}