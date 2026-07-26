using Application.Common.Exceptions;
using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Repositories.ClinicianRepositories;
using Domain.Entities.ClinicianEntities;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Queries.ReadClinicianQuery;

public class ReadClinicianQueryHandler(IClinicianRepository clinicianRepository)
    : IRequestHandler<ReadClinicianQuery, ClinicianDetailedOutputModel>
{
    public async Task<ClinicianDetailedOutputModel> Handle(ReadClinicianQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking clinician
        var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken,
            clinician => clinician.ClinicianCategories,
            clinician => clinician.Appointments,
            clinician => clinician.AppointmentProtocols,
            clinician => clinician.Results);
        if (clinician == null)
            throw new NotFoundException(nameof(Clinician), request.Id);
        
        // Returning output model
        return new ClinicianDetailedOutputModel(
            clinician, 
            clinician.ClinicianCategories, 
            clinician.Appointments, 
            clinician.AppointmentProtocols, 
            clinician.Results);
    }
}