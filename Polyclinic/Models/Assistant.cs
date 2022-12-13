﻿using Polyclinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class Assistant
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [ForeignKey("PolyclinicUser")]
        public string? PolyclinicUserID { get; set; }
        public PolyclinicUser? PolyclinicUser { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
    }
}
