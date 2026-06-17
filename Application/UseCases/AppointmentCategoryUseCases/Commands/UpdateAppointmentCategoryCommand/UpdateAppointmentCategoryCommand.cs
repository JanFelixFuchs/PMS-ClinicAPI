using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.AppointmentOutputModels;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.AppointmentCategoryUseCases.Commands.UpdateAppointmentCategoryCommand;

public record UpdateAppointmentCategoryCommand(
    Guid Id,
    string Name,
    string Abbreviation,
    string Color) 
    : IRequest<AppointmentCategoryDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}