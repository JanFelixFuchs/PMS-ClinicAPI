using Application.Common.OutputModels.PatientOutputModels;
using Application.Common.Transactions;
using Application.Repositories.PatientRepositories;
using Domain.Entities.PatientEntities;
using MediatR;

namespace Application.UseCases.PatientUseCases.Commands.CreatePatientCommand;

public class CreatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePatientCommand, PatientDetailedOutputModel>
{
    public async Task<PatientDetailedOutputModel> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteAsync(async () =>
        {
            // Creating patient
            var patient = new Patient(
                request.Clinic,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender,
                request.Street,
                request.HouseNumber,
                request.City,
                request.ZipCode,
                request.Country,
                request.Email,
                request.PhoneNumber,
                request.InsuranceStatus,
                request.Allergies,
                request.Remarks
            );
            
            // Adding patient
            await patientRepository.AddAsync(patient, cancellationToken);
            
            // Returning output model
            return new PatientDetailedOutputModel(
                patient, 
                patient.Appointments, 
                patient.AppointmentProtocols, 
                patient.Results);
        }, cancellationToken);
    }
}