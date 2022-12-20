using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Polyclinic.Models.Identity
{
    [Index("NormalizedEmail", Name = "EmailIndex")]
    [Index("NormalizedUserName", Name = "UserNameIndex", IsUnique = true)]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Assistants = new HashSet<Assistant>();
            Doctors = new HashSet<Doctor>();
            FunctionalDiagnosticsDoctors = new HashSet<FunctionalDiagnosticsDoctor>();
            Patients = new HashSet<Patient>();
            Roles = new HashSet<AspNetRole>();
        }

        [Key]
        public string Id { get; set; } = null!;
        [StringLength(256)]
        public string UserName { get; set; } = null!;
        [StringLength(256)]
        public string NormalizedUserName { get; set; } = null!;
        [StringLength(256)]
        public string Email { get; set; } = null!;
        [StringLength(256)]
        public string NormalizedEmail { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string SecurityStamp { get; set; } = null!;
        public string ConcurrencyStamp { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [InverseProperty("PolyclinicUser")]
        public virtual ICollection<Assistant> Assistants { get; set; }
        [InverseProperty("PolyclinicUser")]
        public virtual ICollection<Doctor> Doctors { get; set; }
        [InverseProperty("PolyclinicUser")]
        public virtual ICollection<FunctionalDiagnosticsDoctor> FunctionalDiagnosticsDoctors { get; set; }
        [InverseProperty("PolyclinicUser")]
        public virtual ICollection<Patient> Patients { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
