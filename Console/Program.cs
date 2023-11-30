using Domain.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Console.Configurations;
using Application.Configurations;

#if DEBUG
args = [];
#endif

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.Development.json", true);

var configuration = builder.Build();

var serviceProvider = new ServiceCollection()
.AddDependencyInjectionConfiguration(configuration)
.AddApplicationServices(configuration)
.BuildServiceProvider();

var sender = serviceProvider.GetService<ISender>() ?? new Mediator(serviceProvider);
await sender.Send(new StartConsoleCommand(args));