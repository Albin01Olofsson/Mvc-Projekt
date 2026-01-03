using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class EditProjectViewModel
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = " Vänligen ange titel ")]
        public string Title { get; set; }

        [Required(ErrorMessage = " Vänligen ange beskrivning ")]
        [MinLength(10, ErrorMessage = " Beskrivningen måste vara minst 10 tecken lång ")]
        public string Description { get; set; }
    }
}

