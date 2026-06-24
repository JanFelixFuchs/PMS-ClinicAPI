using Application.Common.OutputModels.IdentityOutputModels;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.LoginUserCommand;

public record LoginUserCommand(
    string Code,
    string Username,
    string Password)
    : IRequest<(LoginUserOutputModel Payload, string RefreshToken)>;