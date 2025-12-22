using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektApp.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

    
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
