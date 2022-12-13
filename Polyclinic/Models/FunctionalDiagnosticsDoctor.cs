using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class FunctionalDiagnosticsDoctor
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public PolyclinicUser? PolyclinicUser { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
    }
}
