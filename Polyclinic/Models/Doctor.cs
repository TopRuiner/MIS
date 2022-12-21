using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Отчетсво")]
        public string MiddleName { get; set; }
        [Column(TypeName = "date")]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("PolyclinicUser")]
        [Display(Name = "Id пользователя")]
        public string? PolyclinicUserID { get; set; }
        public virtual PolyclinicUser? PolyclinicUser { get; set; }
        [Display(Name = "Специальнсть")]
        public string Speciality { get; set; }
        [Display(Name = "Категория")]
        public string? Category { get; set; }
        [Display(Name = "Степень")]
        public string? Degree { get; set; }
        [NotMapped]
        public string ReturnDateForDisplay
        {
            get
            {
                return this.BirthDate?.ToShortDateString();
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
        public string ReturnFIOAndSpeciality
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName + " (" + Speciality + ")";
            }
        }
    }
}
