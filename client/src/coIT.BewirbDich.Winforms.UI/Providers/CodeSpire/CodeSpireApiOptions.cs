using coIT.BewirbDich.Winforms.UI.Configuration;
using FluentValidation;

namespace coIT.BewirbDich.Winforms.UI.Providers.CodeSpire;

public sealed record CodeSpireApiOptions
{
    public const string ConfigurationSectionKey = "CodeSpireApi";

    public string Url { get; init; } = string.Empty;
}

internal sealed class CodeSpireOptionsValidator : OptionsValidator<CodeSpireApiOptions>
{
    public CodeSpireOptionsValidator()
    {
        RuleFor(options => options.Url).NotEmpty();
    }
}