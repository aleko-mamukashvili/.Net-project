using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class City
{
    [Required]
    public int CityId { get; set; }
    [MaxLength(50)]
    public required string CityName { get; set; }
}
