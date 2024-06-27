namespace APBD9_pracadomowa.DTOs;

public class PatientInfoDTO
{
    public int IdPatient { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }

    public ICollection<PrescriptionDTO> Prescriptions { get; set; }
}