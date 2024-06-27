using System.Transactions;
using APBD9_pracadomowa.DTOs;
using APBD9_pracadomowa.Models;
using APBD9_pracadomowa.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD9_pracadomowa.Controllers;

[Route("api/prescription")]
[ApiController]
public class PrescriptionController : ControllerBase
{

    private IDbServices _dbServices;

    public PrescriptionController(IDbServices dbServices)
    {
        _dbServices = dbServices;
    }

    [HttpPost]
    public async Task<IActionResult> PrescriptionForPatient(GetPrescriptionForPatient getPrescriptionForPatient)
    {
        if (await _dbServices.DoesPatientExist(getPrescriptionForPatient.Patient.IdPatient))
        {
           await _dbServices.AddPatient(getPrescriptionForPatient);
        }

        if (await _dbServices.DoesMedicationsExist(getPrescriptionForPatient.Medicaments))
        {
            return NotFound("Medication does not exist");
        }

        if (await _dbServices.IsAbove10Medication(getPrescriptionForPatient.Medicaments))
        {
            return NotFound("Above 10 Medications");
        }

        if (await _dbServices.IsDueDateLessDate(getPrescriptionForPatient))
        {
            return NotFound("DueDate is less then Date");
        }
        

        Prescription prescription = new Prescription()
        {
            Date = getPrescriptionForPatient.Date,
            DueDate = getPrescriptionForPatient.DueDate,
            IdPatient = getPrescriptionForPatient.Patient.IdPatient
        };

        List<PrescriptionMedicament> prescriptionMedicaments = new List<PrescriptionMedicament>();
        
        foreach (var med in getPrescriptionForPatient.Medicaments)
        {
            prescriptionMedicaments.Add(new PrescriptionMedicament()
            {
                Prescription = prescription,
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Details = med.Description
            });
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        { 
            await _dbServices.AddPatient(getPrescriptionForPatient);
            await _dbServices.AddPrescription(prescription);
            //await _dbServices.AddPrescriptionmedicaments(prescriptionMedicaments);
        }
        
    
        return Created();
    }
    
    
}