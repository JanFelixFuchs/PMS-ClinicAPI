using Domain.Common.Utils.Constants;

namespace Domain.Common.Utils.Validation;

public static class PasswordValidationPolicy
{
    public static void Validate(string rawPassword)
    {
        PropertyValidationHelper.ConstructPropertyValidation(
            PropertyValidationConditions.IsMatchingRegex(rawPassword, RegexPatterns.Password, "Password"));
    }
}