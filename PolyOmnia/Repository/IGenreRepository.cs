using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre?> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);

        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds);
        Task<Genre?> GetGenreWithVideosAsync(int genreId);

        Task<IEnumerable<Genre>> GetAllWithVideoCountAsync();
    }
}
