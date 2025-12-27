using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CV
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CVId { get; set; }
        
        public string Education { get; set; }
        
        public string Experience { get; set; }

        public string? Skills { get; set; }



        [ForeignKey("Profile")]
        public int ProfileId { get; set; }

        public Profile Profile { get; set; }

    }
}
