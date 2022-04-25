using System.ComponentModel.DataAnnotations;
namespace wasmdiv;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class OperatorValidation : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return (value != null) && (((double)value) > 0);
    }
}