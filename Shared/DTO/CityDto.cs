using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public class CityDto
{
    [MaxLength(50)]
    public string? Name { get; set; }
}
