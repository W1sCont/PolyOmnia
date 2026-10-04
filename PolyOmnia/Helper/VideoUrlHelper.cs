using System.Text.RegularExpressions;

namespace PolyOmnia.Helpers
{
    public static class VideoUrlHelper
    {

        public static string ConvertToEmbedUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            // Перевіряємо, чи це посилання на YouTube
            var youtubeMatch = Regex.Match(
                url,
                @"(?:youtu\.be/|youtube\.com/(?:embed/|v/|watch\?v=|watch\?.+&v=))([\w-]{11})",
                RegexOptions.IgnoreCase);

            if (youtubeMatch.Success)
            {
                string videoId = youtubeMatch.Groups[1].Value;
                return $"https://www.youtube.com/embed/{videoId}";
            }

            // Якщо це не YouTube (наприклад, прямий .mp4 файл за посиланням), повертаємо як є
            return url;
        }

        public static bool IsYouTubeUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url) &&
                   (url.Contains("youtube.com") || url.Contains("youtu.be"));
        }

        public static string GetThumbnailUrl(string videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                return string.Empty;

            var match = Regex.Match(videoUrl, @"(?:youtu\.be/|youtube\.com/(?:embed/|v/|watch\?v=|watch\?.+&v=))([\w-]{11})");
            if (match.Success)
            {
                string videoId = match.Groups[1].Value;
                return $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg";
            }

            return string.Empty;
        }
    }
}