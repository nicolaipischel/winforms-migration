using FluentValidation;

namespace CodeSpire.Api.Features.IssueDocument;

internal sealed class IssueDocumentValidator : Validator<IssueDocumentRequest>
{
    public IssueDocumentValidator()
    {
        RuleFor(x => x.VersicherungscheinAusgestellt)
            .Must(x => true);
    }
}