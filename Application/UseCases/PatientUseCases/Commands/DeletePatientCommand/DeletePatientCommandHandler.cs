using Application.Common.Exceptions;
using Application.Common.Transactions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Commands.DeletePatientCommand;

public class DeletePatientCommandHandler(
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
                throw new NotFoundException(nameof(Patient), request.Id);
            
            // Deleting patient
            patient.Delete(patient.Appointments, patient.AppointmentProtocols, patient.Results);
        }, cancellationToken);
    }
}