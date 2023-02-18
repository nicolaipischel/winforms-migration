using FluentValidation;

namespace CodeSpire.Api.Features.AddDocument;


internal sealed class AddDocumentValidator : Validator<AddDocumentRequest>
{
    public AddDocumentValidator()
    {
        RuleFor(x => x.ZusatzschutzAufschlag)
            .GreaterThan(0)
            .When(x => x.InkludiereZusatzschutz);
    }
}

