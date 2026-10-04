using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email або Логін")]
        [Display(Name = "Email або Логін")]
        public string EmailOrLogin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запом'ятати мене")]
        public bool RememberMe { get; set; }
    }
}
