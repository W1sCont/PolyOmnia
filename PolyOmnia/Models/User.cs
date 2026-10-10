using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyOmnia.Models
{
    public enum UserRole
    {
        Unconfirmed = 1,
        User = 2,
        Moderator = 3,
        Admin = 4
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsPublished { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
