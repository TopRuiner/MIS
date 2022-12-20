using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    [NotMapped]
    public class AssignUserToARoleModel
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
    }
}
