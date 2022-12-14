using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public PolyclinicUser? PolyclinicUser { get; set; }
        [ForeignKey("Polis")]
        public int PolisID { get; set; }
        public Polis Polis { get; set; }
        public int SnilsNumber { get; set; }
        public string? WorkPlace { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
        public IEnumerable<Inspection>? Inspections { get; set; }
        [NotMapped]
        public string ReturnDateForDisplay
        {
            get
            {
                return this.BirthDate?.ToShortDateString();
            }
        }

    }
}
