using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class DoctorAppointment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientId { get; set; }
        [Display(Name = "Пациент")]

        public virtual Patient? Patient { get; set; }
        [Display(Name = "Номер кабинета")]
        public string? CabinetId { get; set; }
        [Display(Name = "Дата приема")]
        public DateTime? DateTime { get; set; }
        [Display(Name = "Статус")]
        public string? Status { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Врач")]
        public int? DoctorId { get; set; }
        [Display(Name = "Врач")]

        public virtual Doctor? Doctor { get; set; }

    }
}
