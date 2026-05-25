using Domain.Commons.Value_Objects;

namespace Application.Common.OutputModels.SharedOutputModels;

public class ContactInformationOutputModel(ContactInformation contactInformation)
{
    public string Email { get; init; } = contactInformation.Email;
    public string PhoneNumber { get; init; } = contactInformation.PhoneNumber;
}