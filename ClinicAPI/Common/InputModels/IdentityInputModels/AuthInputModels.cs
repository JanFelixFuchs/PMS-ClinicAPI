using System.ComponentModel.DataAnnotations;
using Domain.Commons.Enums;

namespace PMS_ClinicAPI.Common.InputModels.IdentityInputModels;

public record RegisterClinicInputModel(
    [Required] string Code,
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
    [Required] string PhoneNumber,
    [Required] string Username,
    [Required] string Password,
    [Required] string RoleNameWithNoRights,
    [Required] string RoleNameWithAllRights);

public record LoginUserInputModel(
    [Required] string Code,
    [Required] string Username,
    [Required] string Password);

public record RefreshTokensInputModel(
    [Required] string AccessToken);

public record UpdateClinicCodeInputModel(
    [Required] string OldCode,
    [Required] string NewCode);

public record UpdateUsernameInputModel(
    [Required] string OldUsername, 
    [Required] string NewUsername);

public record UpdatePasswordInputModel(
    [Required] string OldPassword, 
    [Required] string NewPassword);