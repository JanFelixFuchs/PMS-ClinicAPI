using Domain.Common.Interfaces;

namespace Domain.Tests.Utils.Models.Interfaces;

public record TestArchivable(bool IsArchived) : IArchivable
{
    public bool IsArchived { get; } = IsArchived;
}