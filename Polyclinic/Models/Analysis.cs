using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Analysis
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        [ForeignKey("Assistant")]
        public int AssistantId { get; set; }
        public virtual Assistant? Assistant { get; set; }

    }
}
