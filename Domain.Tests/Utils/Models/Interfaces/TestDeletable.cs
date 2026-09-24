using Domain.Common.Interfaces;

namespace Domain.Tests.Utils.Models.Interfaces;

public record TestDeletable(bool IsDeleted) : IDeletable
{
    public bool IsDeleted { get; } = IsDeleted;
}