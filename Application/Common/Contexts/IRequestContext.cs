namespace Application.Common.Contexts;

public interface IRequestContext
{
    Guid ClinicId { get; }
    Guid UserId { get; }
}