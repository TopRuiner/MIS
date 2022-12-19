using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class AnalysisReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        public string? DiagnosisId { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public string? Type { get; set; }
        [ForeignKey("Assistant")]
        public int? AssistantId { get; set; }
        public virtual Assistant? Assistant { get; set; }
        public string? СabinetNum { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
