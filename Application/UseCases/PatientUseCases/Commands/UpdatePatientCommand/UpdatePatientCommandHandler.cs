using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.Transactions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.PatientUseCases.Commands.UpdatePatientCommand;

public class UpdatePatientCommandHandler(
    ILogger<UpdatePatientCommandHandler> logger,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePatientCommand, PatientDetailedOutputModel>
{
    public async Task<PatientDetailedOutputModel> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
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
            
            // Updating patient
            patient.Update(
                request.LastName,
                request.Street,
                request.HouseNumber,
                request.City,
                request.ZipCode,
                request.Country,
                request.Email,
                request.PhoneNumber,
                request.InsuranceStatus,
                request.Allergies,
                request.Remarks);
            
            // Returning output model
            return new PatientDetailedOutputModel(
                patient, 
                patient.Appointments, 
                patient.AppointmentProtocols, 
                patient.Results);
        }, cancellationToken);
    }
}