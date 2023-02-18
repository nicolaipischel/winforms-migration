using CodeSpire.Application;
using CodeSpire.Domain;

namespace CodeSpire.Api.Features;

/// <summary>
/// Response containing the requested Document.
/// </summary>
internal sealed record AddDocumentRequest
{
    /// <summary>
    /// The Type of the document
    /// </summary>
    public Dokumenttyp Typ { get; set; }
    
    /// <summary>
    /// The calculation method
    /// </summary>
    public Berechnungsart Berechnungsart { get; set; }
    
    /// <summary>
    /// The value used for the calculation.
    /// </summary>
    public decimal Berechnungbasis { get; set; }

    /// <summary>
    /// Value indicating whether additional protection is wanted.
    /// </summary>
    public bool InkludiereZusatzschutz { get; set; }
    
    /// <summary>
    /// The percentage amount that is added on top for additional protection.
    /// </summary>
    public float ZusatzschutzAufschlag { get; init; }
    
    /// <summary>
    /// Value indicating if the holder has a webshop.
    /// </summary>
    public bool HatWebshop { get; set; }

    /// <summary>
    /// The risk that is estimated for the holder.
    /// </summary>
    public Risiko Risiko { get; set; }
    
    /// <summary>
    /// The contribution amount
    /// </summary>
    public decimal Beitrag { get; set; }

    /// <summary>
    /// Value indicating whether a policy has been issued.
    /// </summary>
    public bool VersicherungsscheinAusgestellt { get; set; }
    
    /// <summary>
    /// The insured amount.
    /// </summary>
    public decimal Versicherungssumme { get; set; }
}

// ReSharper disable once UnusedType.Global
internal sealed class AddDocumentEndpoint : Endpoint<AddDocumentRequest>
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
            Typ = req.Typ,
            Versicherungssumme = req.Versicherungssumme,
            HatWebshop = req.HatWebshop,
            InkludiereZusatzschutz = req.InkludiereZusatzschutz,
            VersicherungsscheinAusgestellt = req.VersicherungsscheinAusgestellt,
            ZusatzschutzAufschlag = req.ZusatzschutzAufschlag
        };
        document.Kalkuliere();
        _repo.Add(document);
        return SendOkAsync(ct);
    }
}