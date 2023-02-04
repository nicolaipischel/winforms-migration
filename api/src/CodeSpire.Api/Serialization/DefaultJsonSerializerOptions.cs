using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeSpire.Api.Serialization;

internal static class DefaultJsonSerializerOptions
{
    private static readonly JsonConverter[] Converters = Array.Empty<JsonConverter>();
    
    public static void ConfigureJsonConverters(this JsonSerializerOptions options)
    {
        var converters = options.Converters;
        converters.Add(new JsonStringEnumConverter());
    
        foreach (var converter in Converters) converters.Add(converter);
    }
}

