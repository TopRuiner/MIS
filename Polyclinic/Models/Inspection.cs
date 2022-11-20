using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Inspection
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public string? Complaint { get; set; }
        public string? Data { get; set; }
        public string? Appointment { get; set; }
        [ForeignKey("Diagnosis")]
        public int? DiagnosisId { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        public DateTime? Date { get; set; }


    }
}
