using Application.Common.Exceptions;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientQuery;

public class ReadPatientQueryHandler(IPatientRepository patientRepository)
    : IRequestHandler<ReadPatientQuery, PatientDetailedOutputModel>
{
    public async Task<PatientDetailedOutputModel> Handle(ReadPatientQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking patient
        var patient = await patientRepository.GetByClinicIdAndPatientIdAsync(
            request.Clinic.Id, 
            request.Id, 
            cancellationToken,
            patient => patient.Appointments,
            patient => patient.AppointmentProtocols,
            patient => patient.Results);
        if (patient == null)
            throw new NotFoundException(nameof(Patient), request.Id);
        
        // Returning output model
        return new PatientDetailedOutputModel(
            patient, 
            patient.Appointments, 
            patient.AppointmentProtocols, 
            patient.Results);
    }
}