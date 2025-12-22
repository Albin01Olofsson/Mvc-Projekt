using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektApp.Models
{
    public class UserSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserSkillId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Skill")]
        public int SkillId { get; set; }

        public User User { get; set; }
        public Skill Skill { get; set; }
    }
}
