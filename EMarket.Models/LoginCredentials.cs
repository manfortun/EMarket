using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class LoginCredentials
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}
