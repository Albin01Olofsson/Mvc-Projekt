using System.ComponentModel.DataAnnotations;
namespace ProjektApp.Viewmodels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Namn måste anges")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Bio måste anges")]
        [MinLength(10)]
        public string Bio { get; set; }

        public bool IsPrivate { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
