using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.ListDocument;

/// <summary>
/// Response containing all available documents.
/// </summary>
internal sealed record ListDocumentsResponse
{
    public IReadOnlyCollection<Dokument> Documents { get; init; } = new List<Dokument>();
}