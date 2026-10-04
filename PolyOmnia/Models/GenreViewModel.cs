using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class CreateGenreViewModel
    {
        [Required(ErrorMessage = "Вкажіть назву жанру/категорії")]
        [StringLength(50, ErrorMessage = "Назва не повинна перевищувати 50 символів")]
        public string Name { get; set; } = string.Empty;
    }

    public class EditGenreViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть назву жанру/категорії")]
        [StringLength(50, ErrorMessage = "Назва не повинна перевищувати 50 символів")]
        public string Name { get; set; } = string.Empty;
    }
}