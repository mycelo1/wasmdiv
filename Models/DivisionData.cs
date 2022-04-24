using System.ComponentModel.DataAnnotations;

namespace wasmdiv.Models;

public class DivisionData
{
    [Required] public double Dividend { get; set; }
    [Required] public double Divisor { get; set; }
    [Required] public int DecimalDigits { get; set; }
}