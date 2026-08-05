using Domain.Entities.IdentityEntities;

namespace Application.Common.Behaviours.RequestContextBehaviour;

public interface IRequireRequestContext
{
    // Properties
    Clinic Clinic { get; set; }
    User User { get; set; }   
}