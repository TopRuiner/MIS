using System.ComponentModel.DataAnnotations;

namespace Polyclinic.Models
{
    public class FunctionalDiagnosticsDoctor
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
    }
}
