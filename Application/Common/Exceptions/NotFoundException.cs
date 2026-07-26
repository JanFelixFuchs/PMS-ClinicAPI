using System.Net;
using Utils.Exceptions.Base;
using Utils.Exceptions.Errors.Types;

namespace Application.Common.Exceptions;

public class NotFoundException : CustomExceptionBase
{
    public NotFoundException(string typeName, Guid id)
        : base(
            $"{typeName} with id {id} not found", 
            HttpStatusCode.NotFound, 
            ErrorType.NOT_FOUND) { }

    public NotFoundException(string typeName, ICollection<Guid> ids)
        : base(
            $"{typeName} with ids [{string.Join(", ", ids)}] not found", 
            HttpStatusCode.NotFound, 
            ErrorType.NOT_FOUND) { }
}