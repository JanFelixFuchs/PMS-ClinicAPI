using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using MediatR;

namespace Application.UseCases.AppointmentUseCases.Queries.ReadAppointmentsQuery;

public class ReadAppointmentsQueryHandler(
    IAppointmentRepository appointmentRepository)
    : IRequestHandler<ReadAppointmentsQuery, List<AppointmentOverviewOutputModel>>
{
    public async Task<List<AppointmentOverviewOutputModel>> Handle(ReadAppointmentsQuery request, CancellationToken cancellationToken)
    {
        // Querying appointments
        var appointments = await appointmentRepository.GetByClinicIdAndDatesAsync(
            request.Clinic.Id, 
            request.StartDate,
            request.EndDate, 
            cancellationToken);
        
        // Returning output model
        return appointments
            .Select(appointment => new AppointmentOverviewOutputModel(appointment))
            .ToList();
    }
}