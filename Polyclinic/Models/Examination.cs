using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Examination
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Тип")]
        public string Type { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [ForeignKey("FunctionalDiagnosticsDoctor")]
        [Display(Name = "Врач функциональной диагностики")]
        public int FunctionalDiagnosticsDoctorId { get; set; }
        public virtual FunctionalDiagnosticsDoctor? FunctionalDiagnosticsDoctor { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

    }
}
