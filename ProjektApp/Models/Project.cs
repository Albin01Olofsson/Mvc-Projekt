using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektApp.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
