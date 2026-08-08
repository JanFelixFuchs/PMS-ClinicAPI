using Domain.Common.Enums;
using Utils.Exceptions.CustomExceptions;
using Utils.Exceptions.Errors.Codes;
using Utils.Exceptions.Errors.Field;

namespace Domain.Common.Utils.Helper;

public static class FileHelper
{
    public static AppendixContentType InferFileContentType(byte[] file, string propertyName)
    {
        return file switch
        {
            [0x25, 0x50, 0x44, 0x46, ..] => AppendixContentType.Pdf,
            [0xFF, 0xD8, 0xFF, ..] => AppendixContentType.Jpeg,
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, ..] => AppendixContentType.Png,
            _ => throw new ValidationException(
                $"{propertyName} must be of a supported content type", 
                [new FieldError(propertyName, ErrorCode.UNSUPPORTED_FILE_TYPE)])
        };
    }
}