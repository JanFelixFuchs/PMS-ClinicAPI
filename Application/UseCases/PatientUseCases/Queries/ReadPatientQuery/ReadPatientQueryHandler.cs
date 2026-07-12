using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.PatientUseCases.Queries.ReadPatientQuery;

public class ReadPatientQueryHandler(
    ILogger<ReadPatientQueryHandler> logger,
    IPatientRepository patientRepository)
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
        {
            logger.LogWarning(LogMessages.EntityNotFound, nameof(patient), request.Id);
            throw new NotFoundException(nameof(Patient), request.Id);
        }
        
        // Returning output model
        return new PatientDetailedOutputModel(
            patient, 
            patient.Appointments, 
            patient.AppointmentProtocols, 
            patient.Results);
    }
}