using Application.Common.OutputModels.ClinicianOutputModels;
using Application.Repositories.ClinicianRepositories;
using MediatR;

namespace Application.UseCases.ClinicianUseCases.Queries.ReadCliniciansQuery;

public class ReadCliniciansQueryHandler(IClinicianRepository clinicianRepository)
    : IRequestHandler<ReadCliniciansQuery, List<ClinicianOverviewOutputModel>>
{
    public async Task<List<ClinicianOverviewOutputModel>> Handle(ReadCliniciansQuery request, CancellationToken cancellationToken)
    {
        // Querying clinician
        var clinicians = await clinicianRepository.GetByClinicIdAsync(
            request.Clinic.Id, 
            request.Archived,
            cancellationToken);
        
        // Returning output model
        return clinicians
            .Select(clinician => new ClinicianOverviewOutputModel(clinician))
            .ToList();
    }
}