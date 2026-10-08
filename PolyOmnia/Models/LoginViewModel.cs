using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Err_EmailOrLogin")]
        [Display(Name = "Label_EmailOrLogin")]
        public string EmailOrLogin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Err_EnterPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Label_Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Label_RememberMe")]
        public bool RememberMe { get; set; }
    }
}
