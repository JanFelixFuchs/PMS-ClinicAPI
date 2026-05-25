using Domain.Commons.Enums;
using Domain.Commons.Value_Objects;

namespace Application.Common.OutputModels.SharedOutputModels;

public class AddressOutputModel(Address address)
{
    public string Street { get; init; } = address.Street;
    public string HouseNumber { get; init; } = address.HouseNumber;
    public string City { get; init; } = address.City;
    public string ZipCode { get; init; } = address.ZipCode;
    public Country Country { get; init; } = address.Country;
}