using Application.Common.Exceptions;
using Application.Common.Logging;
using Application.Common.Transactions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.PatientUseCases.Commands.DeletePatientCommand;

public class DeletePatientCommandHandler(
    ILogger<DeletePatientCommandHandler> logger,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePatientCommand>
{
    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(async () =>
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
            
            // Deleting patient
            patient.Delete(patient.Appointments, patient.AppointmentProtocols, patient.Results);
        }, cancellationToken);
    }
}