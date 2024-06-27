using System.ComponentModel.DataAnnotations;

namespace APBD9_pracadomowa.Models;

public class AppUser
{
    [Key]
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
}