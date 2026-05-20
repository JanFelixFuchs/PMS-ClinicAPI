using Domain.Entities.IdentityEntities;

namespace Application.Common.Behaviours.RequestContextBehaviour;

public interface IRequireRequestContext
{
    Clinic Clinic { get; set; }
    User User { get; set; }   
}