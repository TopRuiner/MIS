using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Diagnosis
    {
        [Key]
        [ForeignKey("Inspection")]
        public int Id { get; set; }
        public string MKB { get; set; }
        public string MyProperty { get; set; }
        public string Description { get; set; }
    }
}
