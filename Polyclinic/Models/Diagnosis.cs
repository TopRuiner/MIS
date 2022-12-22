using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Diagnosis
    {
        [Key]
        [Display(Name = "Код МКБ")]
        public string Id { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        [NotMapped]
        public string ReturnIdAndDescription
        {
            get
            {
                return Id + " " + Description;
            }
        }
    }
}
