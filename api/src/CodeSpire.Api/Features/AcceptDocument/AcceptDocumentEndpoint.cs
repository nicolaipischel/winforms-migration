using CodeSpire.Domain.Interfaces;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.AcceptDocument;

// ReSharper disable once UnusedType.Global
internal sealed class AcceptDocumentEndpoint : Endpoint<AcceptDocumentRequest>
{
    private readonly IRepository _repo;

    public AcceptDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Patch("/documents/{id}/accept");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces(200)
             .Produces(404)
             .Produces(400);
        });
        Summary(s =>
        {
            s.Summary = "Accepts an already existing offer (turning document into a policy)";
            s.RequestParam(r => r.Typ, "The document type which you want to transition to");
            s.ExampleRequest = new AcceptDocumentRequest { Typ = Dokumenttyp.Versicherungsschein };
            s.Responses[200] = "Offer has been accepted and document turned into an insurance policy";
            s.Responses[404] = "The document you wanted to accept does not exist";
            s.Responses[400] = "You did something which was against the validation rules";
        });
    }

    public override Task HandleAsync(AcceptDocumentRequest req, CancellationToken ct)
    {
        var documentId = Route<Guid>("id");
        var docToUpdate = _repo.Find(documentId);
        
        if (docToUpdate is null) return SendNotFoundAsync(ct);
        
        docToUpdate = docToUpdate with { Typ = req.Typ };
        
        _repo.Update(docToUpdate);

        return SendNoContentAsync(ct);
    }
}