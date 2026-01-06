using System.ComponentModel.DataAnnotations;

namespace ProjektApp.Viewmodels
{
    public class SendMessageViewModel
    {
        public int ReceiverProfileId { get; set; }

        [Required(ErrorMessage = "Meddelandet får inte vara tomt")]
        public string Content { get; set; }


        public string? SenderName { get; set; }
    }
}
