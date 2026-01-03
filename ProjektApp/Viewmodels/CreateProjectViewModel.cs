using System.ComponentModel.DataAnnotations;
namespace ProjektApp.Viewmodels
{
    public class CreateProjectViewModel
    {
        [Required(ErrorMessage = "Vänligen ange titel")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Beskrivning Saknas")]
        [MinLength(10, ErrorMessage = "Beskrivning måste vara minst 10 tecken")]
        public string Description { get; set; }
    }
}
