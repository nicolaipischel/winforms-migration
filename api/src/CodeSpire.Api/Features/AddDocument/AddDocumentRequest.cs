using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features.AddDocument;

/// <summary>
/// Response containing the requested Document.
/// </summary>
internal sealed record AddDocumentRequest
{
    /// <summary>
    /// The calculation method
    /// </summary>
    public Berechnungsart Berechnungsart { get; init; }

    /// <summary>
    /// Value indicating whether additional protection is wanted.
    /// </summary>
    public bool InkludiereZusatzschutz { get; init; }
    
    /// <summary>
    /// The percentage amount that is added on top for additional protection.
    /// </summary>
    public float ZusatzschutzAufschlag { get; init; }
    
    /// <summary>
    /// Value indicating if the holder has a webshop.
    /// </summary>
    public bool HatWebshop { get; init; }

    /// <summary>
    /// The risk that is estimated for the holder.
    /// </summary>
    public Risiko Risiko { get; init; }

    /// <summary>
    /// The insured amount.
    /// </summary>
    public decimal Versicherungssumme { get; init; }
}