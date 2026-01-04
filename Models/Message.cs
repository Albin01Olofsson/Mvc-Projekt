using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; } = false;
        public string? SenderId { get; set; }
        public string ReceiverId { get; set; }

        public User? Sender { get; set; }
        public User Receiver { get; set; }

        public string? SenderName { get; set; }


    }
}
