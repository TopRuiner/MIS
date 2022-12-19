using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Examination
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        [ForeignKey("FunctionalDiagnosticsDoctor")]
        public int FunctionalDiagnosticsDoctorId { get; set; }
        public virtual FunctionalDiagnosticsDoctor? FunctionalDiagnosticsDoctor { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

    }
}
