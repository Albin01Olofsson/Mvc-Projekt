using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        [Required(ErrorMessage =  " Vänligen ange namn ")]
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string? PictureUrl { get; set; }

        public bool IsPrivate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }
       
    }
}


