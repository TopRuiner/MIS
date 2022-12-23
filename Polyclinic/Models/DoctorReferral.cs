using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class DoctorReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        [Display(Name = "Диагноз")]
        public string? DiagnosisId { get; set; }
        [Display(Name = "Диагноз")]

        public virtual Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Доктор выписавший направление")]
        public int? DoctorIdInitial { get; set; }
        [Display(Name = "Доктор выписавший направление")]

        public virtual Doctor? DoctorInitial { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientId { get; set; }
        [Display(Name = "Пациент")]

        public virtual Patient? Patient { get; set; }
        [Display(Name = "Тип")]
        public string? Type { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Доктор")]
        public int DoctorIdTarget { get; set; }
        [Display(Name = "Доктор")]

        public virtual Doctor? DoctorTarget { get; set; }
        [Display(Name = "Номер кабинета")]
        public string? СabinetNum { get; set; }
        [Display(Name = "Дата")]
        public DateTime? DateTime { get; set; }
    }
}
