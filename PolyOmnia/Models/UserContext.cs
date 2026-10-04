using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

// Add-Migration
// Update-Database

namespace PolyOmnia.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Video> Videos { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
    }
}
