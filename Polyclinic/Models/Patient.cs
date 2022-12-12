using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public int UserID { get; set; }
        [ForeignKey("Polis")]
        public int PolisID { get; set; }
        public Polis Polis { get; set; }
        public int SnilsNumber { get; set; }
        public string? WorkPlace { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
        public IEnumerable<Inspection>? Inspections { get; set; }

    }
}
