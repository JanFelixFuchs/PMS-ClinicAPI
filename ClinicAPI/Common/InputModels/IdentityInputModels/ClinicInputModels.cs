using System.ComponentModel.DataAnnotations;
using Domain.Commons.Enums;

namespace PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

public record UpdateClinicInputModel(
    [Required] string Name,
    [Required] string Abbreviation,
    [Required] string Owner,
    [Required] MedicalField? MedicalField,
    [Required] string Street,
    [Required] string HouseNumber,
    [Required] string City,
    [Required] string ZipCode,
    [Required] Country? Country,
    [Required] string Email,
    [Required] string PhoneNumber);