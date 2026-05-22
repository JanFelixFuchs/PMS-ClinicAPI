using Domain.Commons.Utils.Constants;

namespace Domain.Commons.Utils.Validation;

public static class PasswordValidationPolicy
{
    public static void Validate(string rawPassword)
    {
        ValidationHelper.ConstructPropertyValidation(
            ValidationConditions.IsMatchingRegex(rawPassword, RegexPatterns.Password, "Password"));
    }
}