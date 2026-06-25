using Application.Common.OutputModels.IdentityOutputModels;
using MediatR;

namespace Application.UseCases.AuthUseCases.Commands.RefreshTokensCommand;

public record RefreshTokensCommand(
    string RefreshToken)
    : IRequest<(RefreshTokensOutputModel Payload, string RefreshToken)>;