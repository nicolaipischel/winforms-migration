using FluentValidation;
using Microsoft.Extensions.Options;

namespace coIT.BewirbDich.Winforms.UI.Configuration;

internal class OptionsValidator<TOptions> : AbstractValidator<TOptions>, IValidateOptions<TOptions> where TOptions : class
{
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        var result = Validate(options);
        if (result.IsValid) return ValidateOptionsResult.Success;

        var errors = result.Errors.Select(error => error.ErrorMessage);
        return ValidateOptionsResult.Fail(errors);
    }
}