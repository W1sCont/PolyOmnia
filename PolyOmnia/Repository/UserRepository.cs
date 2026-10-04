using Microsoft.EntityFrameworkCore;
using PolyOmnia.Models;

namespace PolyOmnia.Repository
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly IPasswordHasher _passwordHasher;
        public UserRepository(UserContext context, IPasswordHasher passwordHasher) : base(context) { _passwordHasher = passwordHasher; }

        public async Task<User?> CreateUserAsync(RegisterViewModel model)
        {
            var (passwordHash, passwordSalt) = _passwordHasher.HashPassword(model.Password);
            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Login = model.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.User
            };
            await _context.Users.AddAsync(user);
            return user;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        }
        public async Task<bool> IsLoginUniqueAsync(string login)
        {
            return !await _context.Users.AsNoTracking().AnyAsync(u => u.Login == login);
        }

        public async Task<User?> GetByLoginOrEmailAsync(string emailOrLogin)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == emailOrLogin || u.Email == emailOrLogin);
        }
        public async Task UpdateRoleAsync(int userId, UserRole newRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Role = newRole;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.AsNoTracking().Where(u => u.Role == role).ToListAsync();
        }
        public Task<User?> GetUserWithVideosAsync(int userId)
        {
            return _context.Users.AsNoTracking().Include(u => u.Videos).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
