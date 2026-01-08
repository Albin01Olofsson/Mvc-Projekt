using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class EditCVViewModel
    {
        public int CVId { get; set; }

        [Display(Name = "Utbildning")]
        [Required(ErrorMessage = "vänligen ange utbildning")]
        public string Education { get; set; }
        [Display(Name = "Erfarenhet")]
        [Required(ErrorMessage = "väligen ange erfarenheter")]
        public string Experience { get; set; }
        [Display(Name = "Kompetenser")]
        public string? Skills { get; set; }
    }
}
