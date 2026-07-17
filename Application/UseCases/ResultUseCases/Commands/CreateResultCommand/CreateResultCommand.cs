using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ResultUseCases.Commands.CreateResultCommand;

public record CreateResultCommand(
    string Title,
    DateTime DateOfCreation,
    byte[] Appendix,
    string? Remarks,
    Guid PatientId,
    Guid ClinicianId,
    Guid? DeviceId)
    : IRequest<ResultDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}