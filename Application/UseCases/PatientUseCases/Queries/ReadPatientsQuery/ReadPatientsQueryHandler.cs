using Application.Common.OutputModels.PatientOutputModels;
using Application.Repositories.PatientRepositories;
using MediatR;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientsQuery;

public class ReadPatientsQueryHandler(IPatientRepository patientRepository) 
    : IRequestHandler<ReadPatientsQuery, List<PatientOverviewOutputModel>>
{
    public async Task<List<PatientOverviewOutputModel>> Handle(ReadPatientsQuery request, CancellationToken cancellationToken)
    {
        // Querying patients
        var patients = await patientRepository.GetByClinicIdAsync(
            request.Clinic.Id,
            request.Archived,
            cancellationToken);
        
        // Returning output model
        return patients
            .Select(patient => new PatientOverviewOutputModel(patient))
            .ToList();
    }
}