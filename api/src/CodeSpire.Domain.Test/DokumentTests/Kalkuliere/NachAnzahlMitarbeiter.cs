using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Domain.Test.DokumentTests.Kalkuliere;

public sealed class NachAnzahlMitarbeiter
{
    private static readonly Guid FixedDocumentId = Guid.NewGuid();
    
    [Theory]
    [MemberData(nameof(GeneriereTestDatenFuerAnzahlMitarbeiter))]
    public void BerechnetVersicherungbeitragUndBasis(
        Berechnungsart berechnungsart,
        double versicherungsumme,
        Risiko risiko,
        bool hatWebshop,
        bool inkludiereZusatzschutz,
        float zusatzaufschlagInProzent,
        double erwarteterBeitrag,
        double erwarteteBerechnungsBasis)
    {
        // Arrange.
        var actual = new Dokument(FixedDocumentId)
        {
            Berechnungsart = berechnungsart,
            Risiko = risiko,
            HatWebshop = hatWebshop,
            Typ = Dokumenttyp.Angebot,
            Versicherungssumme = (decimal)versicherungsumme,
            InkludiereZusatzschutz = inkludiereZusatzschutz,
            ZusatzschutzAufschlag = zusatzaufschlagInProzent,
            VersicherungsscheinAusgestellt = false,
        };

        // Act.
        actual.Kalkuliere();
        
        // Assert.
        actual.Beitrag.Should().Be((decimal)erwarteterBeitrag);
        actual.Berechnungbasis.Should().Be((decimal)erwarteteBerechnungsBasis);
    }
    
    public static IEnumerable<object[]> GeneriereTestDatenFuerAnzahlMitarbeiter()
    {
        yield return new object[] { Berechnungsart.AnzahlMitarbeiter, 100000, Risiko.Mittel, true, true, 20f, 31200, 100 };
        yield return new object[] { Berechnungsart.AnzahlMitarbeiter, 5000000, Risiko.Gering, true, true, 25f, 1250000, 5000 };
        yield return new object[] { Berechnungsart.AnzahlMitarbeiter, 5000000, Risiko.Gering, true, false, 0f, 1000000, 5000 };
        yield return new object[] { Berechnungsart.AnzahlMitarbeiter, 5000000, Risiko.Gering, false, true, 20f, 1200000, 5000 };
        yield return new object[] { Berechnungsart.AnzahlMitarbeiter, 200000, Risiko.Gering, false, true, 25f, 50000, 200 };
    }
}