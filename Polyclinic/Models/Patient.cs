using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }
        [Column(TypeName = "date")]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }
        [ForeignKey("PolyclinicUser")]
        [Display(Name = "Id пользователя")]

        public string? PolyclinicUserID { get; set; }
        [Display(Name = "Id пользователя")]

        public virtual PolyclinicUser? PolyclinicUser { get; set; }
        [Display(Name = "Номер полиса")]
        public int PolisID { get; set; }
        [Display(Name = "Компания полиса")]
        public string PoilsCompany { get; set; }
        [Column(TypeName = "date")]
        [Display(Name = "Дата окончания полиса")]
        public DateTime? PolisEndDate { get; set; }
        [Display(Name = "Номер снилса")]
        public int SnilsNumber { get; set; }
        [Display(Name = "Место работы")]
        public string? WorkPlace { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
        public IEnumerable<Examination>? Examinations { get; set; }
        public IEnumerable<Inspection>? Inspections { get; set; }
        [NotMapped]
        public string? ReturnBirthDateForDisplay
        {
            get
            {
                return this.BirthDate?.ToShortDateString();
            }
        }
        [NotMapped]
        public string? ReturnPolisEndDateForDisplay
        {
            get
            {
                return this.PolisEndDate?.ToShortDateString();
            }
        }
        [NotMapped]
        public string? ReturnFIO
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName;
            }
        }
        [NotMapped]
        public string? ReturnFIOAndBirthDate
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName + " " + this.BirthDate?.ToShortDateString();
            }
        }

    }
}
