using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ім'я має бути від 2 до 30 символів")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть прізвище")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Прізвище має бути від 2 до 30 символів")]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть логін")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Логін має бути від 3 до 30 символів")]
        [Display(Name = "Логін")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше 6 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження пароля")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
