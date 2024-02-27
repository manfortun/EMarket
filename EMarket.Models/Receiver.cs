using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Receiver
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;

    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = default!;

    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = default!;

    [Required]
    [DisplayName("Contact No.")]
    [StringLength(11)]
    public string ContactNo { get; set; } = default!;

    [Required]
    [DisplayName("Complete Address")]
    public string Address { get; set; } = default!;
}
