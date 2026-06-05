using System.ComponentModel.DataAnnotations;
using Domain.Commons.Enums;

namespace PMS_ClinicAPI.Common.InputModels.PatientInputModels;

public record CreatePatientInputModel(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateTime? DateOfBirth,
    [Required] Gender? Gender,
    [Required] string Street,
    [Required] string HouseNumber,
    [Required] string City,
    [Required] string ZipCode,
    [Required] Country? Country,
    [Required] string Email,
    [Required] string PhoneNumber,
    [Required] InsuranceStatus? InsuranceStatus,
    string? Allergies,
    string? Remarks);
    
public record UpdatePatientInputModel(
    [Required] string LastName,
    [Required] string Street,
    [Required] string HouseNumber,
    [Required] string City,
    [Required] string ZipCode,
    [Required] Country? Country,
    [Required] string Email,
    [Required] string PhoneNumber,
    [Required] InsuranceStatus? InsuranceStatus,
    string? Allergies,
    string? Remarks);