namespace CodeSpire.Domain.Models;

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
    public decimal Berechnungbasis { get; set; }

    public bool InkludiereZusatzschutz { get; set; }
    public float ZusatzschutzAufschlag { get; set; }
    
    //Gibt es nur bei Unternehmen, die nach Umsatz abgerechnet werden
    public bool HatWebshop { get; init; }

    public Risiko Risiko { get; set; }
    
    public decimal Beitrag { get; set; }

    public bool VersicherungsscheinAusgestellt { get; init; }
    public decimal Versicherungssumme { get; init; }

    public Dokument(Guid id)
    {
        Id = id;
    }

    public void Kalkuliere()
    {
        //Versicherungsnehmer, die nach Haushaltssumme versichert werden (primär Vereine) stellen immer ein mittleres Risiko da
        if (Berechnungsart == Berechnungsart.Haushaltssumme)
        {
            Risiko = Risiko.Mittel;
        }

        //Versicherungsnehmer, die nach Anzahl Mitarbeiter abgerechnet werden und mehr als 5 Mitarbeiter haben, können kein Lösegeld absichern
        if (Berechnungsart == Berechnungsart.AnzahlMitarbeiter)
            if (Berechnungbasis > 5)
            {
                InkludiereZusatzschutz = false;
                ZusatzschutzAufschlag = 0;
            }

        //Versicherungsnehmer, die nach Umsatz abgerechnet werden, mehr als 100.000€ ausweisen und Lösegeld versichern, haben immer mittleres Risiko
        if (Berechnungsart == Berechnungsart.Umsatz)
            if (Berechnungbasis > 100000m && InkludiereZusatzschutz)
            {
                Risiko = Risiko.Mittel;
            }

        decimal beitrag;
        switch (Berechnungsart)
        {
            case Berechnungsart.Umsatz:
                Berechnungbasis = (decimal) Math.Pow((double)Versicherungssumme, 0.25d);
                beitrag = 1.1m * Berechnungbasis;
                if (HatWebshop) //Webshop gibt es nur bei Unternehmen, die nach Umsatz abgerechnet werden
                    beitrag *= 2;
                break;
            case Berechnungsart.Haushaltssumme:
                Berechnungbasis = (decimal)Math.Log10((double) Versicherungssumme);
                beitrag = 1.0m * Berechnungbasis + 100m;
                break;
            case Berechnungsart.AnzahlMitarbeiter:
                Berechnungbasis = Versicherungssumme / 1000;

                if (Berechnungbasis < 4)
                    beitrag = Berechnungbasis * 250m;
                else
                    beitrag = Berechnungbasis * 200m;

                break;
            default:
                throw new Exception();
        }

        if (InkludiereZusatzschutz)
            beitrag *= 1.0m + (decimal) ZusatzschutzAufschlag / 100.0m;

        if (Risiko == Risiko.Mittel)
        {
            if (Berechnungsart is Berechnungsart.Haushaltssumme or Berechnungsart.Umsatz)
                beitrag *= 1.2m;
            else
                beitrag *= 1.3m;
        }

        Berechnungbasis = Math.Round(Berechnungbasis, 2);
        Beitrag = Math.Round(beitrag, 2);
    }
}