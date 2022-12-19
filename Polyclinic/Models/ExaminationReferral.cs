using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class ExaminationReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        public string? DiagnosisId { get; set; }
        public virtual Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public string? Type { get; set; }
        [ForeignKey("FunctionalDiagnosticsDoctor")]
        public int? FunctionalDiagnosticsDoctorId { get; set; }
        public virtual FunctionalDiagnosticsDoctor? FunctionalDiagnosticsDoctor { get; set; }
        public string? СabinetNum { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
