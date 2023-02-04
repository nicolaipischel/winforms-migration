
using CodeSpire.Api.Client;
using coIT.BewirbDich.Winforms.UI.Providers.CodeSpire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace coIT.BewirbDich.Winforms.UI;

internal static class DependencyInjection
{
    public static IServiceCollection AddApiClient(this IServiceCollection services, IConfiguration config)
    {
        var configSection = config.GetSection(CodeSpireApiOptions.ConfigurationSectionKey);
        var options = configSection.Get<CodeSpireApiOptions>();
        services.Configure<CodeSpireApiOptions>(configSection).AddSingleton<IValidateOptions<CodeSpireApiOptions>, CodeSpireOptionsValidator>();;
        
        services.AddHttpClient(nameof(CodeSpireApiClient), c =>
        {
            c.DefaultRequestHeaders.Add("Accept", "application/json");
            c.DefaultRequestHeaders.Add("User-Agent", "coIT.BewirbDich.Winforms/1.0 Client");
        });

        services.AddScoped(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(CodeSpireApiClient));
            return new CodeSpireApiClient(options!.Url, httpClient);
        });

        return services;
    }
}