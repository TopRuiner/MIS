using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class AnalysisReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        [Display(Name = "Предварительный диагноз")]
        public string? DiagnosisId { get; set; }
        [Display(Name = "Предварительный диагноз")]

        public Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Врач")]
        public int? DoctorId { get; set; }
        [Display(Name = "Врач")]

        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientId { get; set; }
        [Display(Name = "Пациент")]

        public virtual Patient? Patient { get; set; }
        [Display(Name = "Тип")]
        public string? Type { get; set; }
        [ForeignKey("Assistant")]
        [Display(Name = "Лаборант")]
        public int? AssistantId { get; set; }
        [Display(Name = "Лаборант")]
        public virtual Assistant? Assistant { get; set; }
        [Display(Name = "Номер кабинета")]
        public string? СabinetNum { get; set; }
        [Display(Name = "Дата")]
        public DateTime? DateTime { get; set; }
    }
}
