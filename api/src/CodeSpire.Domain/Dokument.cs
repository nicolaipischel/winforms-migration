namespace CodeSpire.Domain;

public sealed record Dokument
{
    public Guid Id { get; }

    public Dokumenttyp Typ { get; init; }
    public Berechnungsart Berechnungsart { get; init; }
    /// <summary>
    /// Berechnungsart Umsatz => Jahresumsatz in Euro,
    /// Berechnungsart Haushaltssumme => Haushaltssumme in Euro,
    /// Berechnungsart AnzahlMitarbeiter => Ganzzahl
    /// </summary>
    public decimal Berechnungbasis { get; init; }

    public bool InkludiereZusatzschutz { get; init; }
    public float ZusatzschutzAufschlag { get; init; }
    
    //Gibt es nur bei Unternehmen, die nach Umsatz abgerechnet werden
    public bool HatWebshop { get; init; }

    public Risiko Risiko { get; init; }
    
    public decimal Beitrag { get; init; }

    public bool VersicherungsscheinAusgestellt { get; init; }
    public decimal Versicherungssumme { get; init; }

    public Dokument(Guid id)
    {
        Id = id;
    }
}