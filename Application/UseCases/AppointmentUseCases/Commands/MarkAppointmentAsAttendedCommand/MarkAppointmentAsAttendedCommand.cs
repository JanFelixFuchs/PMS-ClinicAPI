using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Commands.MarkAppointmentAsAttendedCommand;

public record MarkAppointmentAsAttendedCommand(Guid Id) : IRequest<AppointmentDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}