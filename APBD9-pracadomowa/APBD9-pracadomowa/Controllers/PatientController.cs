using APBD9_pracadomowa.DTOs;
using APBD9_pracadomowa.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD9_pracadomowa.Controllers;

[Route("api/patients")]
[ApiController]
public class PatientController : ControllerBase
{
    private DbServices _dbServices;

    public PatientController(DbServices dbServices)
    {
        _dbServices = dbServices;
    }

    [HttpGet("{IdPatient}")]
    public async Task<IActionResult> GetPatientInfo(int IdPatient)
    {

        var allInfo = await _dbServices.GetPatientInfo(IdPatient);
        

        PatientInfoDTO patientInfo = new PatientInfoDTO()
        {
            IdPatient = allInfo.IdPatient,
            FirstName = allInfo.FirstName,
            LastName = allInfo.LastName,
            BirthDate = allInfo.BirthDate,
            Prescriptions = allInfo.Prescriptions.Select(e => new PrescriptionDTO()
            {
                IdPrescription = e.IdPrescription,
                Date = e.Date,
                DueDate = e.DueDate,
                Doctor = new DoctorDTO()
                {
                    IdDoctor = e.Doctor.IdDoctor,
                    FirstName = e.Doctor.FirstName
                },
                Medications = e.PrescriptionMedicaments
                    .Select(e => new MedicamentDTO()
                    {
                        IdMedicament = e.Medicament.IdMedicament,
                        Name = e.Medicament.Name,
                        Dose = e.Dose,
                        Description = e.Details
                    }).ToList()
            }).ToList()
        };
        
        return Ok(patientInfo);
    }
    
    
}