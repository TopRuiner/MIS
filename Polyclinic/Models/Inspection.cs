using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Inspection
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientID { get; set; }
        [Display(Name = "Пациент")]

        public virtual Patient? Patient { get; set; }
        [Display(Name = "Жалобы")]
        public string? Complaint { get; set; }
        [Display(Name = "Рецепт")]
        public string? Recipe { get; set; }

        [ForeignKey("Diagnosis")]
        [Display(Name = "Диагноз")]
        public string? DiagnosisId { get; set; }
        [Display(Name = "Диагноз")]

        public virtual Diagnosis? Diagnosis { get; set; }
        [Display(Name = "Дата проведения")]
        public DateTime? Date { get; set; }
        [Display(Name = "Тип осмотра")]
        public string Type { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Доктор")]
        public int DoctorId { get; set; }
        [Display(Name = "Доктор")]

        public virtual Doctor? Doctor { get; set; }

    }
}
