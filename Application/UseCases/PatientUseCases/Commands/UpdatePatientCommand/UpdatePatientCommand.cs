using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.PatientOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Commands.UpdatePatientCommand;

public record UpdatePatientCommand(
    Guid Id,
    string LastName,
    string Street,
    string HouseNumber,
    string City,
    string ZipCode,
    Country Country,
    string Email,
    string PhoneNumber,
    InsuranceStatus InsuranceStatus,
    string? Allergies,
    string? Remarks)
    : IRequest<PatientDetailedOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}