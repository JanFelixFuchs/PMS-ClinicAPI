using Domain.Common.Utils.Constants;
using Domain.Common.Utils.PropertyValidation;

namespace Domain.Common.Utils.Policies;

public static class PasswordValidationPolicy
{
    public static void Validate(string rawPassword)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsMatchingRegex(rawPassword, RegexPatterns.Password, "Password"));
    }
}