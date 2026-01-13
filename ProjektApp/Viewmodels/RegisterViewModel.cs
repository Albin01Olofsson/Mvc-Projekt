using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class RegisterViewModel
    {

        [Required, EmailAddress]

        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        public string ConfirmPassword { get; set; }
    }
}

