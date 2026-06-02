using Domain.Entities.ClinicianEntities;

namespace Application.Common.OutputModels.ClinicianOutputModels;

public class ClinicianCategoryOverviewOutputModel(ClinicianCategory clinicianCategory)
{
    public Guid Id { get; init; } = clinicianCategory.Id;
    public string Name { get; init; } = clinicianCategory.Name;
    public string Abbreviation { get; init; } = clinicianCategory.Abbreviation;
    public string Color { get; init; } = clinicianCategory.Color;
}

public class ClinicianCategoryDetailedOutputModel(
    ClinicianCategory clinicianCategory,
    ICollection<Clinician> clinicians) 
    : ClinicianCategoryOverviewOutputModel(clinicianCategory)
{
    public ICollection<ClinicianOverviewOutputModel> Clinicians { get; init; } = clinicians.Select(clinician => new ClinicianOverviewOutputModel(clinician)).ToList();
}