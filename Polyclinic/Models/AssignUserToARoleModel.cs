using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Polyclinic.Models
{
    [NotMapped]
    public class AssignUserToARoleModel
    {
        [Display(Name = "Пользователь")]
        public string? UserId { get; set; }
        [Display(Name = "Роль")]
        public string? Role { get; set; }
    }
}
