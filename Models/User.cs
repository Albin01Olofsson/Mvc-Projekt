using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;

        // Navigation
        public Profile Profile { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        

    }
}
