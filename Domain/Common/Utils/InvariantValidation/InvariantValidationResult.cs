using Utils.Exceptions.Errors.Codes;

namespace Domain.Common.Utils.InvariantValidation;

public record InvariantValidationResult(
    bool IsValid,
    string Field,
    ErrorCode ErrorCode,
    string ValidationMessage);