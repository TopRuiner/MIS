using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    [NotMapped]
    public class NotificationEmail
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}
