using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class EditCVViewModel
    {
        public int CVId { get; set; }

        [Required(ErrorMessage = "vänligen ange utbildning")]
        public string Education { get; set; }

        [Required(ErrorMessage ="väligen ange erfarenheter")]
        public string Experience { get; set; }

        public string? Skills { get; set; }
    }
}
