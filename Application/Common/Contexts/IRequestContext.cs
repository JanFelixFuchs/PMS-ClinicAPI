namespace Application.Common.Contexts;

public interface IRequestContext
{
    // Properties
    Guid ClinicId { get; }
    Guid UserId { get; }
}