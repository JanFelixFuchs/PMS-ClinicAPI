using Application.Common.Exceptions;
using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.Transactions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Commands.ArchivePatientCommand;

public class ArchivePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ArchivePatientCommand, PatientDetailedOutputModel>
{
    public async Task<PatientDetailedOutputModel> Handle(ArchivePatientCommand request, CancellationToken cancellationToken)
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
                throw new NotFoundException(nameof(Patient), request.Id);
            
            // Archiving patient
            patient.Archive(patient.Appointments, patient.AppointmentProtocols);
            
            // Returning output model
            return new PatientDetailedOutputModel(
                patient, 
                patient.Appointments, 
                patient.AppointmentProtocols, 
                patient.Results);
        }, cancellationToken);
    }
}