using Application.Common.Exceptions;
using Application.Common.OutputModels.AppointmentOutputModels;
using Application.Repositories.AppointmentRepositories;
using Domain.Entities.AppointmentEntities;
using MediatR;

namespace Application.UseCases.ResultUseCases.Queries.ReadResultQuery;

public class ReadResultQueryHandler(IResultRepository resultRepository)
    : IRequestHandler<ReadResultQuery, ResultDetailedOutputModel>
{
    public async Task<ResultDetailedOutputModel> Handle(ReadResultQuery request, CancellationToken cancellationToken)
    {
        // Querying and checking result
        var result = await resultRepository.GetByClinicIdAndResultIdAsync(
            request.Clinic.Id,
            request.Id,
            cancellationToken,
            result => result.Patient,
            result => result.Clinician,
            result => result.Device);
        if (result == null)
            throw new NotFoundException(nameof(Result), request.Id);
        
        // Returning output model
        return new ResultDetailedOutputModel(result, result.Patient, result.Clinician, result.Device);
    }
}