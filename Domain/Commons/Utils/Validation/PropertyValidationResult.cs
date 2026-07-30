using Utils.Exceptions.Errors.Codes;

namespace Domain.Commons.Utils.Validation;

public record PropertyValidationResult(
    bool IsValid,
    string Field,
    ErrorCode ErrorCode,
    string ValidationMessage);