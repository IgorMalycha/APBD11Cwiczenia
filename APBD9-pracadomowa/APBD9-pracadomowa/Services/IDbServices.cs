using System.Collections.ObjectModel;
using APBD9_pracadomowa.DTOs;
using APBD9_pracadomowa.Models;

namespace APBD9_pracadomowa.Services;

public interface IDbServices
{
    Task<bool> DoesPatientExist(int IdPatient);
    Task AddPatient(GetPrescriptionForPatient getPrescriptionForPatient);
    Task<bool> DoesMedicationsExist(ICollection<GetMedicamentDTO> medicaments);
    
    Task<bool> IsAbove10Medication(ICollection<GetMedicamentDTO> medicaments);
    Task<bool> IsDueDateLessDate(GetPrescriptionForPatient getPrescriptionForPatient);
    
    Task AddPrescription(Prescription prescription);

    Task<Patient> GetPatientInfo(int IdPatient);
    
}