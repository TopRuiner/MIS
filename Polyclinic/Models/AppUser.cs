using System.ComponentModel.DataAnnotations;

namespace Polyclinic.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
    }
}
