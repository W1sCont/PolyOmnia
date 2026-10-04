using Microsoft.AspNetCore.Http;
using PolyOmnia.Models;
using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class CreateVideoViewModel
    {
        [Required(ErrorMessage = "Введіть назву відео")]
        [StringLength(100, ErrorMessage = "Назва не повинна перевищувати 100 символів")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Опис не повинен перевищувати 2000 символів")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Оберіть жанр")]
        public int GenreId { get; set; }

        public VideoSourceType SourceType { get; set; } = VideoSourceType.LocalFile;

        // Поле для файлу (якщо обрано LocalFile)
        public IFormFile? VideoFile { get; set; }

        // Поле для зовнішнього URL (якщо обрано ExternalUrl)
        [Url(ErrorMessage = "Введіть коректну URL-адресу")]
        public string? ExternalUrl { get; set; }

        public IFormFile? ThumbnailFile { get; set; }

        public bool AllowDownload { get; set; } = false;
    }
}