﻿using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public PolyclinicUser? PolyclinicUser { get; set; }
        public string Speciality { get; set; }
        public string? Category { get; set; }
        public string? Degree { get; set; }
    }
}