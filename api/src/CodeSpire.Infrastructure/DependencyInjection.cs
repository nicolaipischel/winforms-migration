using CodeSpire.Domain;
using CodeSpire.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSpire.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Database");
        services.AddSingleton<IRepository, JsonRepository>(x => new JsonRepository(connectionString));

        return services;
    }
}