using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> CreateUserAsync(RegisterViewModel model);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByLoginAsync(string login);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsLoginUniqueAsync(string login);
        Task<User?> GetByLoginOrEmailAsync(string emailOrLogin);
        Task UpdateRoleAsync(int userId, UserRole newRole);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<User?> GetUserWithVideosAsync(int userId);
    }
}
