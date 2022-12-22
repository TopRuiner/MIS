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
        public Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Врач")]
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        [Display(Name = "Тип")]
        public string? Type { get; set; }
        [ForeignKey("Assistant")]
        [Display(Name = "Лаборант")]
        public int? AssistantId { get; set; }
        public virtual Assistant? Assistant { get; set; }
        [Display(Name = "Номер кабинета")]
        public string? СabinetNum { get; set; }
        [Display(Name = "Дата")]
        public DateTime? DateTime { get; set; }
    }
}
