using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Utils.Models.Logging;

public record TestLogEntry(LogLevel LogLevel, string Template);