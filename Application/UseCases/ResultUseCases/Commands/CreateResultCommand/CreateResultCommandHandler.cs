using Application.Common.Exceptions;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.DeviceRepositories;
using Application.Repositories.PatientRepositories;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.PatientEntities;
using MediatR;

namespace Application.UseCases.ResultUseCases.Commands.CreateResultCommand;

public class CreateResultCommandHandler(
    IClinicianRepository clinicianRepository,
    IDeviceRepository deviceRepository,
    IPatientRepository patientRepository,
    IResultRepository resultRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateResultCommand, ResultDetailedOutputModel>
{
    public async Task<ResultDetailedOutputModel> Handle(CreateResultCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Querying and checking patient
            var patient = await patientRepository.GetByClinicIdAndPatientIdAsync(
                request.Clinic.Id,
                request.PatientId,
                cancellationToken);
            if (patient == null)
                throw new NotFoundException(nameof(Patient), request.PatientId);
            
            // Querying and checking clinician
            var clinician = await clinicianRepository.GetByClinicIdAndClinicianIdAsync(
                request.Clinic.Id,
                request.ClinicianId,
                cancellationToken);
            if (clinician == null)
                throw new NotFoundException(nameof(Clinician), request.ClinicianId);

            // Querying and checking device
            Device? device = null;
            if (request.DeviceId is { } deviceId)
            {
                device = await deviceRepository.GetByClinicIdAndDeviceIdAsync(
                    request.Clinic.Id,
                    deviceId,
                    cancellationToken);
                if (device == null)
                    throw new NotFoundException(nameof(Device), deviceId);
            }

            // Creating result
            var result = new Result(
                request.Clinic,
                request.Title,
                request.DateOfCreation,
                request.Appendix,
                request.Remarks,
                patient,
                clinician,
                device);
            
            // Adding result
            await resultRepository.AddAsync(result, cancellationToken);
            
            // Returning output model
            return new ResultDetailedOutputModel(result, patient, clinician, device);
        }, cancellationToken);
    }
}