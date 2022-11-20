using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Polis
    {
        [Key]
        [ForeignKey("Patient")]
        public int Id { get; set; }
        public string Company { get; set; }
        public DateTime EndDate { get; set; }
    }
}
