using System.ComponentModel.DataAnnotations;

namespace wasmdiv.Models;

public class DivisionData
{
    [OperatorValidation]
    public double Dividend { get; set; }

    [OperatorValidation]
    public double Divisor { get; set; }

    [Required]
    public int DecimalDigits { get; set; }
}