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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Column(TypeName = "date")]
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