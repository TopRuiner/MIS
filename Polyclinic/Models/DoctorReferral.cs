using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class DoctorReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        public string? DiagnosisId { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorIdInitial { get; set; }
        public Doctor DoctorInitial { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string? Type { get; set; }
        [ForeignKey("Doctor")]
        public int? DoctorIdTarget { get; set; }
        public Doctor? DoctorTarget { get; set; }
        public string? СabinetNum { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
