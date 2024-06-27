using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBD9_pracadomowa.Models;

[Table("prescriptionsMedicaments")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    
    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; } = null!;

    public int IdPrescription { get; set; }

    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; } = null!;

    public int? Dose { get; set; }

    [MaxLength(100)]
    public string Details { get; set; }
}