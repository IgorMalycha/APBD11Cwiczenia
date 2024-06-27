using APBD9_pracadomowa.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace APBD9_pracadomowa.DTOs;

public class GetPrescriptionForPatient
{
    public PatientDTO Patient { get; set; }

    public ICollection<GetMedicamentDTO> Medicaments { get; set; }

    public DateTime Date { get; set; }
    
    public DateTime DueDate { get; set; }
    
}