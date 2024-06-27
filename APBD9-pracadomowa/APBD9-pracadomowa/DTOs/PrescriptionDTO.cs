namespace APBD9_pracadomowa.DTOs;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }

    public DateTime DueDate { get; set; }
    public ICollection<MedicamentDTO> Medications { get; set; }
    public DoctorDTO Doctor { get; set; }
    
}