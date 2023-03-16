using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.FindDocument;

/// <summary>
/// Response containing the requested Document.
/// </summary>
internal sealed record FindDocumentResponse
{
    /// <summary>
    /// The requested document of null if no matching document was found.
    /// </summary>
    public Dokument? Document { get; init; }
}