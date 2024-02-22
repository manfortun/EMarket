using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Receiver
{
    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = default!;

    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = default!;

    [Required]
    [DisplayName("Contact No.")]
    public string ContactNo { get; set; } = default!;

    [Required]
    [DisplayName("Complete Address")]
    public string Address { get; set; } = default!;

    public string Landmark { get; set; } = default!;
}
