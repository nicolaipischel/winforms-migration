namespace CodeSpire.Api.Features.IssueDocument;

internal sealed record IssueDocumentRequest
{
    /// <summary>
    /// Value indicating if the document has been issued.
    /// </summary>
    public bool VersicherungscheinAusgestellt { get; init; }
}