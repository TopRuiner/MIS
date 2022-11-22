using Microsoft.EntityFrameworkCore;
using Polyclinic.Models;

namespace Polyclinic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Diagnosis>  Diagnoses { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<FunctionalDiagnosticsDoctor> FunctionalDiagnosticsDoctors { get; set; }
        public DbSet<Inspection> Inspections{ get; set; }
        public DbSet<Polis> Polises { get; set; }

    }
}
