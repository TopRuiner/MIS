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
        public string? MiddleName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public virtual PolyclinicUser? PolyclinicUser { get; set; }
        public int PolisID { get; set; }
        public string PoilsCompany { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PolisEndDate { get; set; }
        public int SnilsNumber { get; set; }
        public string? WorkPlace { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
        public IEnumerable<Inspection>? Inspections { get; set; }
        [NotMapped]
        public string ReturnBirthDateForDisplay
        {
            get
            {
                return this.BirthDate?.ToShortDateString();
            }
        }
        [NotMapped]
        public string ReturnPolisEndDateForDisplay
        {
            get
            {
                return this.PolisEndDate?.ToShortDateString();
            }
        }
        [NotMapped]
        public string ReturnFIO
        {
            get
            {
                return FirstName + " " + LastName + " " + MiddleName;
            }
        }
        [NotMapped]
        public string ReturnFIOAndBirthDate
        {
            get
            {
                return FirstName + " " + LastName + " " + MiddleName + " " + this.BirthDate?.ToShortDateString();
            }
        }

    }
}
