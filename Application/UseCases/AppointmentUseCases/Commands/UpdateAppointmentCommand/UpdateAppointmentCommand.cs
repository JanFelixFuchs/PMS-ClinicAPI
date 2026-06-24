using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Commands.UpdateAppointmentCommand;

public record UpdateAppointmentCommand(
    Guid Id,
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    ICollection<Guid> AppointmentCategoryIds,
    Guid PatientId,
    Guid RoomId,
    ICollection<Guid> DeviceIds,
    ICollection<Guid> ClinicianIds)
    : IRequest<AppointmentDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}