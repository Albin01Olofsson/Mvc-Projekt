using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = " Vänligen ange e-postadress ")]
        [EmailAddress(ErrorMessage = " Ogiltig e-postadress ")]
        public string Email { get; set; }
        [Required(ErrorMessage = " Vänligen ange lösenord ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
