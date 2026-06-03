using Application.Common.OutputModels.SharedOutputModels;
using Domain.Commons.Enums;
using Domain.Entities.IdentityEntities;

namespace Application.Common.OutputModels.IdentityOutputModels;

public class ClinicOutputModel(Clinic clinic)
{
    public string Name { get; init; } = clinic.Name;
    public string Abbreviation { get; init; } = clinic.Abbreviation;
    public string Owner { get; init; } = clinic.Owner;
    public MedicalField MedicalField { get; init; } = clinic.MedicalField;
    public AddressOutputModel Address { get; init; } = new(clinic.Address);
    public ContactInformationOutputModel ContactInformation { get; init; } = new(clinic.ContactInformation);
}