using Utils.Exceptions.Errors.Codes;

namespace Domain.Common.Utils.PropertyValidation;

public record PropertyValidationResult(
    bool IsValid,
    string Field,
    ErrorCode ErrorCode,
    string ValidationMessage);