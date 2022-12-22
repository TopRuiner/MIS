using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Analysis
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [Display(Name = "Тип")]
        public string Type { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [ForeignKey("Assistant")]
        [Display(Name = "Лаборант")]
        public int AssistantId { get; set; }
        public virtual Assistant? Assistant { get; set; }

    }
}
