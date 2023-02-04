using CodeSpire.Api.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace coIT.BewirbDich.Winforms.UI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        var hostBuilder = Host.CreateDefaultBuilder();

        hostBuilder.ConfigureServices((context, services) => services.RegisterDependencies(context.Configuration));
        
        var host = hostBuilder.Build();
        var serviceProvider = host.Services;

        Application.Run(serviceProvider.GetRequiredService<Form1>());
    }
}