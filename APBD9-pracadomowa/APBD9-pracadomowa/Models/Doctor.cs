using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD9_pracadomowa.Models;

[Table("doctors")]
public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
}