using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentProtocolUseCases.Commands.UpdateAppointmentProtocolCommand;

public record UpdateAppointmentProtocolCommand(
    Guid Id,
    string? Symptoms,
    string? Diagnosis,
    string? Treatment,
    string? Remarks,
    Guid ClinicianId,
    Guid RoomId,
    ICollection<Guid> DeviceIds)
    : IRequest<AppointmentProtocolDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}