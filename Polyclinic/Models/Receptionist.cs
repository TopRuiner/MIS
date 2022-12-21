﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Receptionist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Column(TypeName = "date")]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public virtual PolyclinicUser? PolyclinicUser { get; set; }
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

    }
}
