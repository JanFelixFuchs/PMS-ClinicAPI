using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;
using MediatR;

namespace Application.UseCases.ClinicUseCases.Commands.UpdateClinicCommand;

public record UpdateClinicCommand(
    string Name,
    string Abbreviation,
    string Owner,
    MedicalField MedicalField,
    string Street,
    string HouseNumber,
    string City,
    string ZipCode,
    Country Country,
    string Email,
    string PhoneNumber) 
    : IRequest<ClinicOutputModel>, IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}