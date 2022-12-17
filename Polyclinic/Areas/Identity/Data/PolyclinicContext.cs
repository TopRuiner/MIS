using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyclinic.Areas.Identity.Data;
using Polyclinic.Models;

namespace Polyclinic.Data;

public class PolyclinicContext : IdentityDbContext<PolyclinicUser>
{
    public PolyclinicContext(DbContextOptions<PolyclinicContext> options)
        : base(options)
    {

    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Analysis> Analyses { get; set; }
    public DbSet<Assistant> Assistants { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Examination> Examinations { get; set; }
    public DbSet<FunctionalDiagnosticsDoctor> FunctionalDiagnosticsDoctors { get; set; }
    public DbSet<Inspection> Inspections { get; set; }
    public DbSet<AnalysisReferral> AnalysisReferrals { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
    public DbSet<DoctorReferral> DoctorReferrals { get; set; }
    public DbSet<ExaminationReferral> ExaminationReferrals { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<PolyclinicUser>
{
    public void Configure(EntityTypeBuilder<PolyclinicUser> builder)
    {
    }
}