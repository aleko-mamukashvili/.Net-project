using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public class PhoneNumberDto
{
    public int PersonId { get; set; }

    [MinLength(4)]
    [MaxLength(50)]
    public required string Number { get; set; }
    [AllowedValues("მობილური", "ოფისის", "სახლის")]
    [MaxLength(10)]
    public required string NumberType { get; set; }
}
