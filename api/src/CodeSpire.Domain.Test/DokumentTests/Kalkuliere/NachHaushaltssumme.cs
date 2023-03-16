using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Domain.Test.DokumentTests.Kalkuliere;

public sealed class NachHaushaltssumme
{
    private static readonly Guid FixedDocumentId = Guid.NewGuid();
    
    [Theory]
    [MemberData(nameof(GeneriereTestDatenFuerHaushaltssumme))]
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
    
    public static IEnumerable<object[]> GeneriereTestDatenFuerHaushaltssumme()
    {
        yield return new object[] { Berechnungsart.Haushaltssumme, 100000, Risiko.Mittel, true, true, 20f, 151.20, 5 };
        yield return new object[] { Berechnungsart.Haushaltssumme, 200000, Risiko.Mittel, true, false, 0f, 126.36, 5.3 };
        yield return new object[] { Berechnungsart.Haushaltssumme, 200000, Risiko.Mittel, false, true, 20f, 151.63, 5.3 };
        yield return new object[] { Berechnungsart.Haushaltssumme, 100000, Risiko.Gering, false, true, 20f, 151.20, 5 };
    }
}