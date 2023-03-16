using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.AcceptDocument;

internal sealed record AcceptDocumentRequest
{ 
    public Dokumenttyp Typ { get; init; }
}