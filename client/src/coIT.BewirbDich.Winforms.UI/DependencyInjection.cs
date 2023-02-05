using CodeSpire.Client;
using coIT.BewirbDich.Winforms.UI.Providers.CodeSpire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace coIT.BewirbDich.Winforms.UI;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddApiClient(config);
        services.AddTransient(sp =>
        {
            var api = sp.GetRequiredService<IApiClient>();
            return new Form1(api);
        });

        return services;
    }
    
    private static IServiceCollection AddApiClient(this IServiceCollection services, IConfiguration config)
    {
        var configSection = config.GetSection(CodeSpireApiOptions.ConfigurationSectionKey);
        var options = configSection.Get<CodeSpireApiOptions>();

        services.AddHttpClient(nameof(ApiClient), c =>
        {
            c.DefaultRequestHeaders.Add("Accept", "application/json");
            c.DefaultRequestHeaders.Add("User-Agent", "coIT.BewirbDich.Winforms/1.0 Client");
        });

        services.AddScoped<IApiClient>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(ApiClient));
            return new ApiClient(options!.Url, httpClient);
        });

        return services;
    }
}