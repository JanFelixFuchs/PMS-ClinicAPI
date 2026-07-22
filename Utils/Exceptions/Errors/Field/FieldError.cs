using Utils.Exceptions.Errors.Codes;

namespace Utils.Exceptions.Errors.Field;

public record FieldError(string Field, ErrorCode ErrorCode);