using Domain.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands;

public class StartConsoleCommandHandler(ISender sender, ILogger<StartConsoleCommandHandler> logger) : IRequestHandler<StartConsoleCommand>
{
    public Task Handle(StartConsoleCommand request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting application");

        if (request.Arguments.Any())
        {
            logger.LogWarning("Не корректно заданы аргументы");
            return Task.CompletedTask;
        }


        return request.Arguments.Select(item => item.ToLower()).ToArray() switch
        {
            _ => throw new Exception("Не определен режим запуска")
        };
    }
}