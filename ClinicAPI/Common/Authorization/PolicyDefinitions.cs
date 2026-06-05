using Domain.Commons.Enums;
using Microsoft.AspNetCore.Authorization;

namespace PMS_ClinicAPI.Common.Authorization;

public static class PolicyDefinitions
{
    public const string CanReadAppointment = "CanReadAppointment";
    public const string CanCreateAppointment = "CanCreateAppointment";
    public const string CanUpdateAppointment = "CanUpdateAppointment";
    public const string CanDeleteAppointment = "CanDeleteAppointment";
    
    public const string CanReadAppointmentCategory = "CanReadAppointmentCategory";
    public const string CanCreateAppointmentCategory = "CanCreateAppointmentCategory";
    public const string CanUpdateAppointmentCategory = "CanUpdateAppointmentCategory";
    public const string CanDeleteAppointmentCategory = "CanDeleteAppointmentCategory";
    
    public const string CanReadAppointmentProtocol = "CanReadAppointmentProtocol";
    public const string CanUpdateAppointmentProtocol = "CanUpdateAppointmentProtocol";
    
    public const string CanReadResult = "CanReadResult";
    public const string CanCreateResult = "CanCreateResult";
    public const string CanDeleteResult = "CanDeleteResult";
    
    public const string CanReadClinician = "CanReadClinician";
    public const string CanCreateClinician = "CanCreateClinician";
    public const string CanUpdateClinician = "CanUpdateClinician";
    public const string CanArchiveClinician = "CanArchiveClinician";
    public const string CanDeleteClinician = "CanDeleteClinician";

    public const string CanReadClinicianCategory = "CanReadClinicianCategory";
    public const string CanCreateClinicianCategory = "CanCreateClinicianCategory";
    public const string CanUpdateClinicianCategory = "CanUpdateClinicianCategory";
    public const string CanDeleteClinicianCategory = "CanDeleteClinicianCategory";
    
    public const string CanReadDevice = "CanReadDevice";
    public const string CanCreateDevice = "CanCreateDevice";
    public const string CanUpdateDevice = "CanUpdateDevice";
    public const string CanArchiveDevice = "CanArchiveDevice";
    public const string CanDeleteDevice = "CanDeleteDevice";

    public const string CanReadDeviceCategory = "CanReadDeviceCategory";
    public const string CanCreateDeviceCategory = "CanCreateDeviceCategory";
    public const string CanUpdateDeviceCategory = "CanUpdateDeviceCategory";
    public const string CanDeleteDeviceCategory = "CanDeleteDeviceCategory";
    
    public const string CanUpdateClinic = "CanUpdateClinic";
    
    public const string CanReadRole = "CanReadRole";
    public const string CanCreateRole = "CanCreateRole";
    public const string CanUpdateRole = "CanUpdateRole";
    public const string CanDeleteRole = "CanDeleteRole";
    
    public const string CanReadUser = "CanReadUser";
    public const string CanCreateUser = "CanCreateUser";
    public const string CanUpdateUser = "CanUpdateUser";
    public const string CanArchiveUser = "CanArchiveUser";
    public const string CanDeleteUser = "CanDeleteUser";
    
    public const string CanReadPatient = "CanReadPatient";
    public const string CanCreatePatient = "CanCreatePatient";
    public const string CanUpdatePatient = "CanUpdatePatient";
    public const string CanArchivePatient = "CanArchivePatient";
    public const string CanDeletePatient = "CanDeletePatient";

    public const string CanReadRoom = "CanReadRoom";
    public const string CanCreateRoom = "CanCreateRoom";
    public const string CanUpdateRoom = "CanUpdateRoom";
    public const string CanArchiveRoom = "CanArchiveRoom";
    public const string CanDeleteRoom = "CanDeleteRoom";
    
    public const string CanReadRoomCategory = "CanReadRoomCategory";
    public const string CanCreateRoomCategory = "CanCreateRoomCategory";
    public const string CanUpdateRoomCategory = "CanUpdateRoomCategory";
    public const string CanDeleteRoomCategory = "CanDeleteRoomCategory";
    
    public static void AddPolicies(AuthorizationOptions authorizationOptions)
    {
        // Appointment
        authorizationOptions.AddPolicy(CanReadAppointment, policy => 
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Appointment, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateAppointment, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Appointment, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateAppointment, policy => 
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Appointment, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteAppointment, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Appointment, ClaimValue.Delete)));
        
        // AppointmentCategory
        authorizationOptions.AddPolicy(CanReadAppointmentCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentCategory, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateAppointmentCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentCategory, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateAppointmentCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentCategory, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteAppointmentCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentCategory, ClaimValue.Delete)));
        
        // AppointmentProtocol
        authorizationOptions.AddPolicy(CanReadAppointmentProtocol, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentProtocol, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanUpdateAppointmentProtocol, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.AppointmentProtocol, ClaimValue.Update)));
        
        // Result
        authorizationOptions.AddPolicy(CanReadResult, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Result, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateResult, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Result, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanDeleteResult, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Result, ClaimValue.Delete)));

        // Clinician
        authorizationOptions.AddPolicy(CanReadClinician, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinician, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateClinician, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinician, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateClinician, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinician, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanArchiveClinician, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinician, ClaimValue.Archive)));
        authorizationOptions.AddPolicy(CanDeleteClinician, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinician, ClaimValue.Delete)));
        
        // ClinicianCategory
        authorizationOptions.AddPolicy(CanReadClinicianCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.ClinicianCategory, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateClinicianCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.ClinicianCategory, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateClinicianCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.ClinicianCategory, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteClinicianCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.ClinicianCategory, ClaimValue.Delete)));
        
        // Device
        authorizationOptions.AddPolicy(CanReadDevice, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Device, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateDevice, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Device, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateDevice, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Device, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanArchiveDevice, policy => 
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Device, ClaimValue.Archive)));
        authorizationOptions.AddPolicy(CanDeleteDevice, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Device, ClaimValue.Delete)));
        
        // DeviceCategory
        authorizationOptions.AddPolicy(CanReadDeviceCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.DeviceCategory, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateDeviceCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.DeviceCategory, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateDeviceCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.DeviceCategory, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteDeviceCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.DeviceCategory, ClaimValue.Delete)));
        
        // Clinic
        authorizationOptions.AddPolicy(CanUpdateClinic, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Clinic, ClaimValue.Update)));
        
        // Role
        authorizationOptions.AddPolicy(CanReadRole, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Role, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateRole, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Role, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateRole, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Role, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteRole, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Role, ClaimValue.Delete)));
        
        // User
        authorizationOptions.AddPolicy(CanReadUser, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.User, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateUser, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.User, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateUser, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.User, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanArchiveUser, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.User, ClaimValue.Archive)));
        authorizationOptions.AddPolicy(CanDeleteUser, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.User, ClaimValue.Delete)));
        
        // Patient
        authorizationOptions.AddPolicy(CanReadPatient, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Patient, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreatePatient, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Patient, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdatePatient, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Patient, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanArchivePatient, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Patient, ClaimValue.Archive)));
        authorizationOptions.AddPolicy(CanDeletePatient, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Patient, ClaimValue.Delete)));
        
        // Room
        authorizationOptions.AddPolicy(CanReadRoom, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Room, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateRoom, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Room, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateRoom, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Room, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanArchiveRoom, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Room, ClaimValue.Archive)));
        authorizationOptions.AddPolicy(CanDeleteRoom, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.Room, ClaimValue.Delete)));
        
        // RoomCategory
        authorizationOptions.AddPolicy(CanReadRoomCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.RoomCategory, ClaimValue.Read)));
        authorizationOptions.AddPolicy(CanCreateRoomCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.RoomCategory, ClaimValue.Create)));
        authorizationOptions.AddPolicy(CanUpdateRoomCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.RoomCategory, ClaimValue.Update)));
        authorizationOptions.AddPolicy(CanDeleteRoomCategory, policy =>
            policy.Requirements.Add(new PolicyRequirement(ClaimType.RoomCategory, ClaimValue.Delete)));
    }
}