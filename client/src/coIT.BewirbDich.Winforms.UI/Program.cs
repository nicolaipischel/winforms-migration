using Microsoft.Extensions.Hosting;

namespace coIT.BewirbDich.Winforms.UI;

static class Program
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

        var builder = Host.CreateDefaultBuilder();
        var app = builder.Build();
        
        ServiceProvider = app.Services;
        
        Application.Run(new Form1());
    }
    internal static IServiceProvider ServiceProvider { get; private set; }
}