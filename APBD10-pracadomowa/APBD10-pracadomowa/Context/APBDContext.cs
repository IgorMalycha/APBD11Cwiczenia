using APBD9_pracadomowa.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD9_pracadomowa.Context;

public class APBDContext : DbContext
{
    public APBDContext()
    {
        
    }
    
    public APBDContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
    }
    
    
    
}