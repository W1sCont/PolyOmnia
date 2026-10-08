using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Reg_EnterName")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Err_FirstNameLength")]
        [Display(Name = "Label_FirstName")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reg_EnterSurname")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Err_LastNameLength")]
        [Display(Name = "Label_LastName")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reg_EnterLogin")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Err_UsernameLength")]
        [Display(Name = "Label_Login")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reg_EnterEmail")]
        [EmailAddress(ErrorMessage = "Err_InvalidEmail")]
        [Display(Name = "Label_Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reg_EnterPassword")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Err_PasswordTooShort")]
        [DataType(DataType.Password)]
        [Display(Name = "Label_Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Reg_ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Err_PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
