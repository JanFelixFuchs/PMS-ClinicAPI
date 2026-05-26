using Domain.Commons.Utils.Constants;
using Domain.Entities.AppointmentEntities;
using Domain.Entities.ClinicianEntities;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using Domain.Entities.PatientEntities;
using Domain.Entities.RoomEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentCategory> AppointmentCategories { get; set; }
    public DbSet<AppointmentProtocol> AppointmentProtocols { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Clinician> Clinicians { get; set; }
    public DbSet<ClinicianCategory> ClinicianCategories { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceCategory> DeviceCategories { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomCategory> RoomCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Invoking base method
        base.OnModelCreating(modelBuilder);
        
        /* - - - Appointment - - - */
        modelBuilder.Entity<Appointment>(entity =>
        {
            // Id
            entity.HasKey(appointment => appointment.Id);
            
            // Clinic
            entity.HasOne(appointment => appointment.Clinic)
                .WithMany(clinic => clinic.Appointments)
                .HasForeignKey(appointment => appointment.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Title
            entity.Property(appointment => appointment.Title)
                .HasMaxLength(Lengths.AppointmentTitle)
                .IsRequired();
            
            // StartTime
            entity.Property(appointment => appointment.StartTime)
                .IsRequired();
            
            // EndTime
            entity.Property(appointment => appointment.EndTime)
                .IsRequired();
            
            // Status
            entity.Property(appointment => appointment.Status)
                .HasMaxLength(Lengths.AppointmentStatus)
                .HasConversion<string>()
                .IsRequired();
            
            // IsDeleted
            entity.Property(appointment => appointment.IsDeleted)
                .IsRequired();
            
            // AppointmentCategories
            entity.HasMany(appointment => appointment.AppointmentCategories)
                .WithMany(appointmentCategory => appointmentCategory.Appointments);
            
            // Patient
            entity.HasOne(appointment => appointment.Patient)
                .WithMany(patient => patient.Appointments)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Room
            entity.HasOne(appointment => appointment.Room)
                .WithMany(room => room.Appointments)
                .HasForeignKey(appointment => appointment.RoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // AppointmentProtocol
            entity.HasOne(appointment => appointment.AppointmentProtocol)
                .WithOne(appointmentProtocol => appointmentProtocol.Appointment)
                .HasForeignKey<AppointmentProtocol>(appointmentProtocol => appointmentProtocol.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Devices
            entity.HasMany(appointment => appointment.Devices)
                .WithMany(device => device.Appointments);
            
            // Clinicians
            entity.HasMany(appointment => appointment.Clinicians)
                .WithMany(clinic => clinic.Appointments);
            
            // Query filter
            entity.HasQueryFilter(appointment => !appointment.IsDeleted);
        });

        /* - - - AppointmentCategory - - - */
        modelBuilder.Entity<AppointmentCategory>(entity =>
        {
            // Id
            entity.HasKey(appointmentCategory => appointmentCategory.Id);
            
            // Clinic
            entity.HasOne(appointmentCategory => appointmentCategory.Clinic)
                .WithMany(clinic => clinic.AppointmentCategories)
                .HasForeignKey(appointmentCategory => appointmentCategory.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(appointmentCategory => appointmentCategory.Name)
                .HasMaxLength(Lengths.CategoryName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(appointmentCategory => appointmentCategory.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // Color
            entity.Property(appointmentCategory => appointmentCategory.Color)
                .HasMaxLength(Lengths.Color)
                .IsRequired();
            
            // IsDeleted
            entity.Property(appointmentCategory => appointmentCategory.IsDeleted)
                .IsRequired();
            
            // Appointments
            entity.HasMany(appointmentCategory => appointmentCategory.Appointments)
                .WithMany(appointment => appointment.AppointmentCategories);
            
            // Query filter
            entity.HasQueryFilter(appointmentCategory => !appointmentCategory.IsDeleted);
        });
        
        /* - - - AppointmentProtocol - - - */
        modelBuilder.Entity<AppointmentProtocol>(entity =>
        {
            // Id
            entity.HasKey(appointmentProtocol => appointmentProtocol.Id);
            
            // Clinic
            entity.HasOne(appointmentProtocol => appointmentProtocol.Clinic)
                .WithMany(clinic => clinic.AppointmentProtocols)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // DateOfAppointment
            entity.Property(appointmentProtocol => appointmentProtocol.DateOfAppointment)
                .IsRequired();
            
            // DateOfProcessingStart
            entity.Property(appointmentProtocol => appointmentProtocol.DateOfProcessingStart);
            
            // DateOfProcessingCompletion
            entity.Property(appointmentProtocol => appointmentProtocol.DateOfProcessingCompletion);

            // Symptoms
            entity.Property(appointmentProtocol => appointmentProtocol.Symptoms)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.Symptoms);
            
            // Diagnosis
            entity.Property(appointmentProtocol => appointmentProtocol.Diagnosis)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.Diagnosis);
            
            // Treatment
            entity.Property(appointmentProtocol => appointmentProtocol.Treatment)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.Treatment);
            
            // Remarks
            entity.Property(appointmentProtocol => appointmentProtocol.Remarks)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.AppointmentProtocolRemarks);
            
            // Status
            entity.Property(appointmentProtocol => appointmentProtocol.Status)
                .HasMaxLength(Lengths.AppointmentProtocolStatus)
                .HasConversion<string>()
                .IsRequired();
            
            // Appointment
            entity.HasOne(appointmentProtocol => appointmentProtocol.Appointment)
                .WithOne(appointment => appointment.AppointmentProtocol)
                .HasForeignKey<AppointmentProtocol>(appointmentProtocol => appointmentProtocol.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Patient
            entity.HasOne(appointmentProtocol => appointmentProtocol.Patient)
                .WithMany(patient => patient.AppointmentProtocols)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.PatientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Clinician
            entity.HasOne(appointmentProtocol => appointmentProtocol.Clinician)
                .WithMany(clinician => clinician.AppointmentProtocols)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.ClinicianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Room
            entity.HasOne(appointmentProtocol => appointmentProtocol.Room)
                .WithMany(room => room.AppointmentProtocols)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.RoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Devices
            entity.HasMany(appointmentProtocol => appointmentProtocol.Devices)
                .WithMany(device => device.AppointmentProtocols);
        });
        
        /* - - - Result - - - */
        modelBuilder.Entity<Result>(entity =>
        {
            // Id
            entity.HasKey(result => result.Id);
            
            // Clinic
            entity.HasOne(result => result.Clinic)
                .WithMany(clinic => clinic.Results)
                .HasForeignKey(result => result.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Title
            entity.Property(result => result.Title)
                .HasMaxLength(Lengths.ResultTitle)
                .IsRequired();
            
            // DateOfCreation
            entity.Property(result => result.DateOfCreation)
                .IsRequired();
            
            // Appendix
            entity.Property(result => result.Appendix)
                .HasColumnType("LONGBLOB")
                .IsRequired();
            
            // AppendixContentType
            entity.Property(result => result.AppendixContentType)
                .HasMaxLength(Lengths.AppendixContentType)
                .HasConversion<string>()
                .IsRequired();
            
            // IsDeleted
            entity.Property(result => result.IsDeleted)
                .IsRequired();
            
            // Remarks
            entity.Property(result => result.Remarks)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.ResultRemarks);

            // Patient
            entity.HasOne(result => result.Patient)
                .WithMany(patient => patient.Results)
                .HasForeignKey(result => result.PatientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Clinician
            entity.HasOne(result => result.Clinician)
                .WithMany(clinician => clinician.Results)
                .HasForeignKey(result => result.ClinicianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Device
            entity.HasOne(result => result.Device)
                .WithMany(device => device.Results)
                .HasForeignKey(result => result.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Query filter
            entity.HasQueryFilter(result => !result.IsDeleted);
        });
        
        /* - - - Clinician - - - */
        modelBuilder.Entity<Clinician>(entity =>
        {
            // Id
            entity.HasKey(clinician => clinician.Id);
            
            // Clinic
            entity.HasOne(clinician => clinician.Clinic)
                .WithMany(clinic => clinic.Clinicians)
                .HasForeignKey(clinician => clinician.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // FirstName
            entity.Property(clinician => clinician.FirstName)
                .HasMaxLength(Lengths.FirstName)
                .IsRequired();
            
            // LastName
            entity.Property(clinician => clinician.LastName)
                .HasMaxLength(Lengths.LastName)
                .IsRequired();
            
            // IsArchived
            entity.Property(clinician => clinician.IsArchived)
                .IsRequired();
            
            // IsDeleted
            entity.Property(clinician => clinician.IsDeleted)
                .IsRequired();
            
            // ClinicianCategories
            entity.HasMany(clinician => clinician.ClinicianCategories)
                .WithMany(clinicianCategory => clinicianCategory.Clinicians);
            
            // User
            entity.HasOne(clinician => clinician.User)
                .WithOne(user => user.Clinician)
                .HasForeignKey<User>(user => user.ClinicianId);
            
            // Appointments
            entity.HasMany(clinician => clinician.Appointments)
                .WithMany(appointment => appointment.Clinicians);
            
            // AppointmentProtocols
            entity.HasMany(clinician => clinician.AppointmentProtocols)
                .WithOne(appointmentProtocol => appointmentProtocol.Clinician)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.ClinicianId)
                .IsRequired();
            
            // Results
            entity.HasMany(clinician => clinician.Results)
                .WithOne(result => result.Clinician)
                .HasForeignKey(result => result.ClinicianId)
                .IsRequired();
            
            // Query filter
            entity.HasQueryFilter(clinician => !clinician.IsDeleted);
        });

        /* - - - ClinicianCategory - - - */
        modelBuilder.Entity<ClinicianCategory>(entity =>
        {
            // Id
            entity.HasKey(clinicianCategory => clinicianCategory.Id);
            
            // Clinic
            entity.HasOne(clinicianCategory => clinicianCategory.Clinic)
                .WithMany(clinic => clinic.ClinicianCategories)
                .HasForeignKey(clinicianCategory => clinicianCategory.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(clinicianCategory => clinicianCategory.Name)
                .HasMaxLength(Lengths.CategoryName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(clinicianCategory => clinicianCategory.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // Color
            entity.Property(clinicianCategory => clinicianCategory.Color)
                .HasMaxLength(Lengths.Color)
                .IsRequired();
            
            // IsDeleted
            entity.Property(clinicianCategory => clinicianCategory.IsDeleted)
                .IsRequired();
            
            // Clinicians
            entity.HasMany(clinicianCategory => clinicianCategory.Clinicians)
                .WithMany(clinician => clinician.ClinicianCategories);
            
            // Query filter
            entity.HasQueryFilter(clinicianCategory => !clinicianCategory.IsDeleted);
        });
        
        /* - - - Device - - - */
        modelBuilder.Entity<Device>(entity =>
        {
            // Id
            entity.HasKey(device => device.Id);
            
            // Clinic
            entity.HasOne(device => device.Clinic)
                .WithMany(clinic => clinic.Devices)
                .HasForeignKey(device => device.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(device => device.Name)
                .HasMaxLength(Lengths.DeviceName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(device => device.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // SerialNumber
            entity.Property(device => device.SerialNumber)
                .HasMaxLength(Lengths.SerialNumber)
                .IsRequired();
            
            // Status
            entity.Property(device => device.Status)
                .HasMaxLength(Lengths.DeviceStatus)
                .HasConversion<string>()
                .IsRequired();
            
            // IsArchived
            entity.Property(device => device.IsArchived)
                .IsRequired();
            
            // IsDeleted
            entity.Property(device => device.IsDeleted)
                .IsRequired();
            
            // DeviceCategories
            entity.HasMany(device => device.DeviceCategories)
                .WithMany(deviceCategory => deviceCategory.Devices);
            
            // Producer
            entity.Property(device => device.Producer)
                .HasMaxLength(Lengths.Producer);
            
            // DateOfPurchase
            entity.Property(device => device.DateOfPurchase);
            
            // DateOfLastMaintenance
            entity.Property(device => device.DateOfLastMaintenance);
            
            // Appointments
            entity.HasMany(device => device.Appointments)
                .WithMany(appointment => appointment.Devices);
            
            // AppointmentProtocols
            entity.HasMany(device => device.AppointmentProtocols)
                .WithMany(appointmentProtocol => appointmentProtocol.Devices);
            
            // Results
            entity.HasMany(device => device.Results)
                .WithOne(result => result.Device)
                .HasForeignKey(result => result.DeviceId);
            
            // Query filter
            entity.HasQueryFilter(device => !device.IsDeleted);
        });
        
        /* - - - DeviceCategory - - - */
        modelBuilder.Entity<DeviceCategory>(entity =>
        {
            // Id
            entity.HasKey(deviceCategory => deviceCategory.Id);
            
            // Clinic
            entity.HasOne(deviceCategory => deviceCategory.Clinic)
                .WithMany(clinic => clinic.DeviceCategories)
                .HasForeignKey(deviceCategory => deviceCategory.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(deviceCategory => deviceCategory.Name)
                .HasMaxLength(Lengths.CategoryName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(deviceCategory => deviceCategory.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // Color
            entity.Property(deviceCategory => deviceCategory.Color)
                .HasMaxLength(Lengths.Color)
                .IsRequired();
            
            // IsDeleted
            entity.Property(deviceCategory => deviceCategory.IsDeleted)
                .IsRequired();
            
            // Devices
            entity.HasMany(deviceCategory => deviceCategory.Devices)
                .WithMany(device => device.DeviceCategories);
            
            // Query filter
            entity.HasQueryFilter(deviceCategory => !deviceCategory.IsDeleted);
        });

        /* - - - Claim - - - */
        modelBuilder.Entity<Claim>(entity =>
        {
            // Id
            entity.HasKey(claim => claim.Id);
            
            // Role
            entity.HasOne(claim => claim.Role)
                .WithMany(role => role.Claims)
                .HasForeignKey(claim => claim.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Type
            entity.Property(claim => claim.Type)
                .HasMaxLength(Lengths.ClaimType)
                .HasConversion<string>()
                .IsRequired();
            
            // Value
            entity.Property(claim => claim.Value)
                .HasMaxLength(Lengths.ClaimValue)
                .HasConversion<string>()
                .IsRequired();
            
            // IsDeleted
            entity.Property(claim => claim.IsDeleted)
                .IsRequired();
            
            // Unique key for roleId and type
            entity.HasIndex(claim => new { claim.RoleId, claim.Type })
                .IsUnique();
            
            // Query filter
            entity.HasQueryFilter(claim => !claim.IsDeleted);
        });
        
        /* - - - Clinic - - - */
        modelBuilder.Entity<Clinic>(entity =>
        {
            // Id
            entity.HasKey(clinic => clinic.Id);
            
            // Code 
            entity.Property(clinic => clinic.Code)
                .HasMaxLength(Lengths.Code)
                .IsRequired();
            
            // NormalizedCode
            entity.Property(clinic => clinic.NormalizedCode)
                .HasMaxLength(Lengths.NormalizedCode)
                .IsRequired();
            
            // Name
            entity.Property(clinic => clinic.Name)
                .HasMaxLength(Lengths.ClinicName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(clinic => clinic.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // Address
            entity.OwnsOne(clinic => clinic.Address, addressProperty =>
            {
                // Street
                addressProperty.Property(address => address.Street)
                    .HasMaxLength(Lengths.Street)
                    .IsRequired();
            
                // HouseNumber
                addressProperty.Property(address => address.HouseNumber)
                    .HasMaxLength(Lengths.HouseNumber)
                    .IsRequired();
                
                // City
                addressProperty.Property(address => address.City)
                    .HasMaxLength(Lengths.City)
                    .IsRequired();
                
                // ZipCode
                addressProperty.Property(address => address.ZipCode)
                    .HasMaxLength(Lengths.ZipCode)
                    .IsRequired();
                
                // Country
                addressProperty.Property(address => address.Country)
                    .HasMaxLength(Lengths.Country)
                    .HasConversion<string>()
                    .IsRequired();
            });
            
            // ContactInformation
            entity.OwnsOne(clinic => clinic.ContactInformation, contactInformationProperty =>
            {
                // Email
                contactInformationProperty.Property(contactInformation => contactInformation.Email)
                    .HasMaxLength(Lengths.Email)
                    .IsRequired();
                
                // PhoneNumber
                contactInformationProperty.Property(contactInformation => contactInformation.PhoneNumber)
                    .HasMaxLength(Lengths.PhoneNumber)
                    .IsRequired();
            });
            
            // Owner
            entity.Property(clinic => clinic.Owner)
                .HasMaxLength(Lengths.Owner)
                .IsRequired();
            
            // MedicalField
            entity.Property(clinic => clinic.MedicalField)
                .HasMaxLength(Lengths.MedicalField)
                .HasConversion<string>()
                .IsRequired();
            
            // Users
            entity.HasMany(clinic => clinic.Users)
                .WithOne(user => user.Clinic)
                .HasForeignKey(user => user.ClinicId)
                .IsRequired();
            
            // Roles
            entity.HasMany(clinic => clinic.Roles)
                .WithOne(role => role.Clinic)
                .HasForeignKey(role => role.ClinicId)
                .IsRequired();
            
            // Devices
            entity.HasMany(clinic => clinic.Devices)
                .WithOne(device => device.Clinic)
                .HasForeignKey(device => device.ClinicId)
                .IsRequired();
            
            // DeviceCategories
            entity.HasMany(clinic => clinic.DeviceCategories)
                .WithOne(deviceCategory => deviceCategory.Clinic)
                .HasForeignKey(deviceCategory => deviceCategory.ClinicId)
                .IsRequired();
            
            // Rooms
            entity.HasMany(clinic => clinic.Rooms)
                .WithOne(room => room.Clinic)
                .HasForeignKey(room => room.ClinicId)
                .IsRequired();
            
            // RoomCategories
            entity.HasMany(clinic => clinic.RoomCategories)
                .WithOne(roomCategory => roomCategory.Clinic)
                .HasForeignKey(roomCategory => roomCategory.ClinicId)
                .IsRequired();
            
            // Appointments
            entity.HasMany(clinic => clinic.Appointments)
                .WithOne(appointment => appointment.Clinic)
                .HasForeignKey(appointment => appointment.ClinicId)
                .IsRequired();
            
            // AppointmentCategories
            entity.HasMany(clinic => clinic.AppointmentCategories)
                .WithOne(appointmentCategory => appointmentCategory.Clinic)
                .HasForeignKey(appointmentCategory => appointmentCategory.ClinicId)
                .IsRequired();
            
            // Clinicians
            entity.HasMany(clinic => clinic.Clinicians)
                .WithOne(clinician => clinician.Clinic)
                .HasForeignKey(clinician => clinician.ClinicId)
                .IsRequired();
            
            // ClinicianCategories
            entity.HasMany(clinic => clinic.ClinicianCategories)
                .WithOne(clinicianCategory => clinicianCategory.Clinic)
                .HasForeignKey(clinicianCategory => clinicianCategory.ClinicId)
                .IsRequired();
            
            // Patients
            entity.HasMany(clinic => clinic.Patients)
                .WithOne(patient => patient.Clinic)
                .HasForeignKey(patient => patient.ClinicId)
                .IsRequired();
            
            // AppointmentProtocols
            entity.HasMany(clinic => clinic.AppointmentProtocols)
                .WithOne(appointmentProtocol => appointmentProtocol.Clinic)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.ClinicId)
                .IsRequired();
            
            // Results
            entity.HasMany(clinic => clinic.Results)
                .WithOne(result => result.Clinic)
                .HasForeignKey(result => result.ClinicId)
                .IsRequired();
            
            // Unique key for normalizedCode
            entity.HasIndex(clinic => clinic.NormalizedCode)
                .IsUnique();
        });
        
        /* - - - Role - - - */
        modelBuilder.Entity<Role>(entity =>
        {
            // Id
            entity.HasKey(role => role.Id);
            
            // Clinic
            entity.HasOne(role => role.Clinic)
                .WithMany(clinic => clinic.Roles)
                .HasForeignKey(role => role.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(role => role.Name)
                .HasMaxLength(Lengths.RoleName)
                .IsRequired();
            
            // NormalizedName
            entity.Property(role => role.NormalizedName)
                .HasMaxLength(Lengths.NormalizedRoleName)
                .IsRequired();
            
            // IsSystemRole
            entity.Property(role => role.IsSystemRole)
                .IsRequired();
            
            // IsDeleted
            entity.Property(role => role.IsDeleted)
                .IsRequired();
            
            // Users
            entity.HasMany(role => role.Users)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId)
                .IsRequired();
            
            // Claims
            entity.HasMany(role => role.Claims)
                .WithOne(claim => claim.Role)
                .HasForeignKey(claim => claim.RoleId)
                .IsRequired();
            
            // Unique key for clinicId, normalizedName and isDeleted
            entity.HasIndex(role => new { role.ClinicId, role.NormalizedName, role.IsDeleted })
                .IsUnique();
            
            // Query filter
            entity.HasQueryFilter(role => !role.IsDeleted);
        });
        
        /* - - - User - - - */
        modelBuilder.Entity<User>(entity =>
        {
            // Id
            entity.HasKey(user => user.Id);
            
            // Clinic
            entity.HasOne(user => user.Clinic)
                .WithMany(clinic => clinic.Users)
                .HasForeignKey(user => user.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Username
            entity.Property(user => user.Username)
                .HasMaxLength(Lengths.Username)
                .IsRequired();
            
            // NormalizedUsername
            entity.Property(user => user.NormalizedUsername)
                .HasMaxLength(Lengths.NormalizedUsername)
                .IsRequired();
            
            // PasswordHash
            entity.Property(user => user.PasswordHash)
                .HasMaxLength(Lengths.PasswordHash)
                .IsRequired();
            
            // IsAdmin
            entity.Property(user => user.IsAdmin)
                .IsRequired();
            
            // IsDeleted
            entity.Property(user => user.IsDeleted)
                .IsRequired();
            
            // RefreshToken
            entity.Property(user => user.RefreshTokenHash)
                .HasMaxLength(Lengths.RefreshTokenHash);
                
            // RefreshTokenExpirationTime
            entity.Property(user => user.RefreshTokenExpirationTime);
            
            // Role
            entity.HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Clinician
            entity.HasOne(user => user.Clinician)
                .WithOne(clinician => clinician.User)
                .HasForeignKey<User>(user => user.ClinicianId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Unique key for clinicId, normalizedUsername and isDeleted
            entity.HasIndex(user => new { user.ClinicId, user.NormalizedUsername, user.IsDeleted })
                .IsUnique();
            
            // Query filter
            entity.HasQueryFilter(user => !user.IsDeleted);
        });
        
        /* - - - Patient - - - */
        modelBuilder.Entity<Patient>(entity =>
        {
            // Id
            entity.HasKey(patient => patient.Id);
            
            // Clinic
            entity.HasOne(patient => patient.Clinic)
                .WithMany(clinic => clinic.Patients)
                .HasForeignKey(patient => patient.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // DateOfCreation
            entity.Property(patient => patient.DateOfCreation)
                .IsRequired();
            
            // FirstName
            entity.Property(patient => patient.FirstName)
                .HasMaxLength(Lengths.FirstName)
                .IsRequired();
            
            // LastName
            entity.Property(patient => patient.LastName)
                .HasMaxLength(Lengths.LastName)
                .IsRequired();
            
            // DateOfBirth
            entity.Property(patient => patient.DateOfBirth)
                .IsRequired();
            
            // Gender
            entity.Property(patient => patient.Gender)
                .HasMaxLength(Lengths.Gender)
                .HasConversion<string>()
                .IsRequired();
            
            // Address
            entity.OwnsOne(patient => patient.Address, addressProperty =>
            {
                // Street
                addressProperty.Property(address => address.Street)
                    .HasMaxLength(Lengths.Street)
                    .IsRequired();
            
                // HouseNumber
                addressProperty.Property(address => address.HouseNumber)
                    .HasMaxLength(Lengths.HouseNumber)
                    .IsRequired();
                
                // City
                addressProperty.Property(address => address.City)
                    .HasMaxLength(Lengths.City)
                    .IsRequired();
                
                // ZipCode
                addressProperty.Property(address => address.ZipCode)
                    .HasMaxLength(Lengths.ZipCode)
                    .IsRequired();
                
                // Country
                addressProperty.Property(address => address.Country)
                    .HasMaxLength(Lengths.Country)
                    .HasConversion<string>()
                    .IsRequired();
            });
            
            // ContactInformation
            entity.OwnsOne(patient => patient.ContactInformation, contactInformationProperty =>
            {
                // Email
                contactInformationProperty.Property(contactInformation => contactInformation.Email)
                    .HasMaxLength(Lengths.Email)
                    .IsRequired();
                
                // PhoneNumber
                contactInformationProperty.Property(contactInformation => contactInformation.PhoneNumber)
                    .HasMaxLength(Lengths.PhoneNumber)
                    .IsRequired();
            });
            
            // InsuranceStatus
            entity.Property(patient => patient.InsuranceStatus)
                .HasMaxLength(Lengths.InsuranceStatus)
                .HasConversion<string>()
                .IsRequired();
            
            // IsArchived
            entity.Property(patient => patient.IsArchived)
                .IsRequired();
            
            // IsDeleted
            entity.Property(patient => patient.IsDeleted)
                .IsRequired();
            
            // Allergies
            entity.Property(patient => patient.Allergies)
                .HasMaxLength(Lengths.Allergies);

            // Remarks
            entity.Property(patient => patient.Remarks)
                .HasColumnType("TEXT")
                .HasMaxLength(Lengths.PatientRemarks);
            
            // Appointments
            entity.HasMany(patient => patient.Appointments)
                .WithOne(appointment => appointment.Patient)
                .HasForeignKey(appointment => appointment.PatientId)
                .IsRequired();
            
            // AppointmentProtocols
            entity.HasMany(patient => patient.AppointmentProtocols)
                .WithOne(appointmentProtocol => appointmentProtocol.Patient)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.PatientId)
                .IsRequired();
            
            // Results
            entity.HasMany(patient => patient.Results)
                .WithOne(result => result.Patient)
                .HasForeignKey(result => result.PatientId)
                .IsRequired();
            
            // Query filter
            entity.HasQueryFilter(patient => !patient.IsDeleted);
        });
        
        /* - - - Room - - - */
        modelBuilder.Entity<Room>(entity =>
        {
            // Id
            entity.HasKey(room => room.Id);
            
            // Clinic
            entity.HasOne(room => room.Clinic)
                .WithMany(clinic => clinic.Rooms)
                .HasForeignKey(room => room.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(room => room.Name)
                .HasMaxLength(Lengths.RoomName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(room => room.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // IsArchived
            entity.Property(roomCategory => roomCategory.IsArchived)
                .IsRequired();
            
            // IsDeleted
            entity.Property(roomCategory => roomCategory.IsDeleted)
                .IsRequired();
            
            // RoomCategories
            entity.HasMany(room => room.RoomCategories)
                .WithMany(roomCategory => roomCategory.Rooms);
            
            // RoomNumber
            entity.Property(room => room.RoomNumber)
                .HasMaxLength(Lengths.RoomNumber);
            
            // Floor
            entity.Property(room => room.Floor)
                .HasMaxLength(Lengths.Floor);
            
            // Building
            entity.Property(room => room.Building)
                .HasMaxLength(Lengths.Building);
            
            // Appointments
            entity.HasMany(room => room.Appointments)
                .WithOne(appointment => appointment.Room)
                .HasForeignKey(appointment => appointment.RoomId)
                .IsRequired();
            
            // AppointmentProtocols
            entity.HasMany(room => room.AppointmentProtocols)
                .WithOne(appointmentProtocol => appointmentProtocol.Room)
                .HasForeignKey(appointmentProtocol => appointmentProtocol.RoomId)
                .IsRequired();
            
            // Query filter
            entity.HasQueryFilter(room => !room.IsDeleted);
        });
        
        /* - - - RoomCategory - - - */
        modelBuilder.Entity<RoomCategory>(entity =>
        {
            // Id
            entity.HasKey(roomCategory => roomCategory.Id);
            
            // Clinic
            entity.HasOne(roomCategory => roomCategory.Clinic)
                .WithMany(clinic => clinic.RoomCategories)
                .HasForeignKey(roomCategory => roomCategory.ClinicId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            // Name
            entity.Property(roomCategory => roomCategory.Name)
                .HasMaxLength(Lengths.CategoryName)
                .IsRequired();
            
            // Abbreviation
            entity.Property(roomCategory => roomCategory.Abbreviation)
                .HasMaxLength(Lengths.Abbreviation)
                .IsRequired();
            
            // Color
            entity.Property(roomCategory => roomCategory.Color)
                .HasMaxLength(Lengths.Color)
                .IsRequired();
            
            // IsDeleted
            entity.Property(roomCategory => roomCategory.IsDeleted)
                .IsRequired();
            
            // Rooms
            entity.HasMany(roomCategory => roomCategory.Rooms)
                .WithMany(room => room.RoomCategories);
            
            // Query filter
            entity.HasQueryFilter(roomCategory => !roomCategory.IsDeleted);
        });
    }
}