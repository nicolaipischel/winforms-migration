using CodeSpire.Domain;
using CodeSpire.Domain.Interfaces;

namespace CodeSpire.Api.Features;

internal sealed class IssueDocumentEndpoint : EndpointWithoutRequest
{
    private readonly IRepository _repo;

    public IssueDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Put("/documents/{id}/issue");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces(200)
             .Produces(404);
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var docToUpdate = _repo.Find(Route<Guid>("id"));

        if (docToUpdate is null) return SendNotFoundAsync(ct);
        
        docToUpdate = docToUpdate with { VersicherungsscheinAusgestellt = true };
        
        _repo.Update(docToUpdate);

        return SendOkAsync(ct);
    }
}