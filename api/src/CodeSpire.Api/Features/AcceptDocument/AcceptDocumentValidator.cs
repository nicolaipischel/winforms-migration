using CodeSpire.Domain.Models;
using FluentValidation;

namespace CodeSpire.Api.Features.AcceptDocument;

internal sealed class AcceptDocumentValidator : Validator<AcceptDocumentRequest>
{
    public AcceptDocumentValidator()
    {
        RuleFor(x => x.Typ)
            .Must(x => x == Dokumenttyp.Versicherungsschein);
    }
}