using CodeSpire.Domain;
using CodeSpire.Domain.Interfaces;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features;

internal sealed class AcceptDocumentEndpoint : EndpointWithoutRequest
{
    private readonly IRepository _repo;

    public AcceptDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Put("/documents/{id}/accept");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces(200)
             .Produces(404);
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var documentId = Route<Guid>("id");
        var docToUpdate = _repo.Find(documentId);
        
        if (docToUpdate is null) return SendNotFoundAsync(ct);
        
        docToUpdate = docToUpdate with { Typ = Dokumenttyp.Versicherungsschein };
        
        _repo.Update(docToUpdate);

        return SendOkAsync(ct);
    }
}