using CodeSpire.Domain.Interfaces;

namespace CodeSpire.Api.Features.IssueDocument;

// ReSharper disable once UnusedType.Global
internal sealed class IssueDocumentEndpoint : Endpoint<IssueDocumentRequest>
{
    private readonly IRepository _repo;

    public IssueDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Patch("/documents/{id}/issue");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces(204)
             .Produces(404);
        });
    }

    public override Task HandleAsync(IssueDocumentRequest req, CancellationToken ct)
    {
        var docToUpdate = _repo.Find(Route<Guid>("id"));

        if (docToUpdate is null) return SendNotFoundAsync(ct);
        
        docToUpdate = docToUpdate with { VersicherungsscheinAusgestellt = req.VersicherungscheinAusgestellt };
        
        _repo.Update(docToUpdate);

        return SendNoContentAsync(ct);
    }
}