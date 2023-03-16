using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Domain.Test.DokumentTests.Kalkuliere;

public sealed class NachUmsatz
{
    private static readonly Guid FixedDocumentId = Guid.NewGuid();
    
    [Theory]
    [MemberData(nameof(GeneriereTestDatenFuerUmsatz))]
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
    
    public static IEnumerable<object[]> GeneriereTestDatenFuerUmsatz()
    {
        yield return new object[] { Berechnungsart.Umsatz, 100000, Risiko.Gering, true, true, 20f, 46.95, 17.78 };
        yield return new object[] { Berechnungsart.Umsatz, 100000, Risiko.Gering, false, true, 20f, 23.47, 17.78 };
        yield return new object[] { Berechnungsart.Umsatz, 100000, Risiko.Gering, true, false, 20f, 39.12, 17.78 };
        yield return new object[] { Berechnungsart.Umsatz, 9990000, Risiko.Gering, true, true, 25f, 154.61, 56.22 };
    }

}