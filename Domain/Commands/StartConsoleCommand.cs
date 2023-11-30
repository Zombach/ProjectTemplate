using MediatR;

namespace Domain.Commands;

public sealed record StartConsoleCommand(IEnumerable<string> Arguments) : IRequest;