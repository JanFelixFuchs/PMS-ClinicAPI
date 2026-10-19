using Application.Common.Behaviours.RequestContextBehaviour;
using Domain.Entities.IdentityEntities;

namespace Application.UnitTests.Utils.Models.Requests;

public class TestRequestWithRequestContext : IRequireRequestContext
{
    public Clinic Clinic { get; set; } = null!;
    public User User { get; set; } = null!;
}