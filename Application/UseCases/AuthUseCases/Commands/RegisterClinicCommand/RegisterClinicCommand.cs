using Application.Common.OutputModels.IdentityOutputModels;
using Domain.Commons.Enums;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.RegisterClinicCommand;

public record RegisterClinicCommand(
    string Code,
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
    string PhoneNumber,
    string Username,
    string Password,
    string RoleNameWithNoRights,
    string RoleNameWithAllRights) 
    : IRequest<(RegisterClinicOutputModel Payload, string RefreshToken)>;