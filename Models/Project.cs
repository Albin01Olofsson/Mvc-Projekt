using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Required(ErrorMessage =  " Vänligen ange titel ")]
        public string Title { get; set; }
        [Required(ErrorMessage =  " Vänligen ange beskrivning ")]
        [MinLength(10, ErrorMessage = " Beskrivningen måste vara minst 10 tecken lång ")]
        public string Description { get; set; }

        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
