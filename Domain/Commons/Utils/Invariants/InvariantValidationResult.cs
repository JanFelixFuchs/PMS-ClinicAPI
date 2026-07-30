namespace Domain.Commons.Utils.Invariants;

public record InvariantValidationResult(
    bool IsValid,
    string ValidationMessage);