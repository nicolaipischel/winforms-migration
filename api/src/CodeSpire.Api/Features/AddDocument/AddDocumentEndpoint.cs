using CodeSpire.Domain.Interfaces;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.AddDocument;

// ReSharper disable once UnusedType.Global
internal sealed partial class AddDocumentEndpoint : Endpoint<AddDocumentRequest>
{
    private readonly IRepository _repo;

    public AddDocumentEndpoint(IRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Post("/documents");
        AllowAnonymous();
        Description(b =>
        {
            b.Accepts<AddDocumentRequest>("application/json")
             .Produces(200);
        });
    }

    public override Task HandleAsync(AddDocumentRequest req, CancellationToken ct)
    {
        var documentId = Guid.NewGuid();
        var document = new Dokument(documentId)
        {
            Berechnungsart = req.Berechnungsart,
            Risiko = req.Risiko,
            Typ = Dokumenttyp.Angebot,
            Versicherungssumme = req.Versicherungssumme,
            HatWebshop = req.HatWebshop,
            InkludiereZusatzschutz = req.InkludiereZusatzschutz,
            VersicherungsscheinAusgestellt = false,
            ZusatzschutzAufschlag = req.ZusatzschutzAufschlag
        };
        document.Kalkuliere();
        _repo.Add(document);
        return SendOkAsync(ct);
    }
}