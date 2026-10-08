using Microsoft.AspNetCore.Http;
using PolyOmnia.Models;

namespace PolyOmnia.Models
{
    public class UserProfileViewModel
    {
        // Інформація про користувача
        public int UserId { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }

        // Статистика
        public long TotalViewsCount => UserVideos.Sum(v => (long)v.ViewCount);

        // Список завантажених відео користувача
        public List<UserVideoItemViewModel> UserVideos { get; set; } = new();
    }

    public class UserVideoItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ThumbnailUrlOrPath { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool AllowDownload { get; set; }
        public string? PrimaryGenreName { get; set; }
    }
}