using Domain.Entities.AppointmentEntities;

namespace Application.Common.OutputModels.AppointmentOutputModels;

public class AppointmentCategoryOverviewOutputModel(AppointmentCategory appointmentCategory)
{
    public Guid Id { get; init; } = appointmentCategory.Id;
    public string Name { get; init; } = appointmentCategory.Name;
    public string Abbreviation { get; init; } = appointmentCategory.Abbreviation;
    public string Color { get; init; } = appointmentCategory.Color;
}

public class AppointmentCategoryDetailedOutputModel(
    AppointmentCategory appointmentCategory,
    ICollection<Appointment> appointments)
    : AppointmentCategoryOverviewOutputModel(appointmentCategory)
{
    public ICollection<AppointmentOverviewOutputModel> Appointments { get; init; } = appointments.Select(appointment => new AppointmentOverviewOutputModel(appointment)).ToList();
}