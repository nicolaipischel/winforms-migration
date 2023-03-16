using CodeSpire.Domain.Interfaces;

namespace CodeSpire.Api.Features.FindDocument;

// ReSharper disable once UnusedType.Global
internal sealed class FindDocumentEndpoint : EndpointWithoutRequest<FindDocumentResponse>
{
    private readonly IRepository _repo;

    public FindDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Get("/documents/{id}");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces<FindDocumentResponse>(200, "application/json")
             .Produces(404);
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var documentId = Route<Guid>("id");
        var document = _repo.Find(documentId);

        if (document is null) return SendNoContentAsync(ct);
        
        Response = new FindDocumentResponse
        {
            Document = document
        };
        
        return Task.CompletedTask;
    }
}