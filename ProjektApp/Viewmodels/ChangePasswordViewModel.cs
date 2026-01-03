using System.ComponentModel.DataAnnotations;
namespace ProjektApp.Viewmodels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nuvarande lösenord")]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt lösenord")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte")]
        [Display(Name = "Bekräfta nytt lösenord")]
        public string ConfirmPassword { get; set; }
    }
}
