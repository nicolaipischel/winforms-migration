using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Domain.Test;

public class DokumentTests
{
    private static readonly Guid FixedDocumentId = Guid.NewGuid();
    private const float AdditionalProtectionPercentage20 = 20f;
    private const float AdditionalProtectionPercentage25 = 25f;
    private const double InsuranceSum100k = 100000;
    private const double InsuranceSum200k = 200000;
    private const double InsuranceSum5M = 5000000;
    private const Dokumenttyp DocType = Dokumenttyp.Angebot;
    private const bool IsInsurancePolicyIssued = false;

    [Theory]
    [InlineData(InsuranceSum100k,Risiko.Mittel, true, true, AdditionalProtectionPercentage20, 151.20, 5)]
    [InlineData(InsuranceSum200k,Risiko.Mittel, true, false, 0, 126.36, 5.3)]
    [InlineData(InsuranceSum200k,Risiko.Mittel, false, true, AdditionalProtectionPercentage20, 151.63, 5.3)]
    [InlineData(InsuranceSum100k,Risiko.Gering, false, true, AdditionalProtectionPercentage20, 151.20, 5)]
    public void Kalkuliere_BerechnungsartHaushaltssumme_ReturnsCorrectlyCalculated(
        double insuranceSum,
        Risiko risk,
        bool hasWebshop,
        bool additionalProtectionRequested,
        float additionalProtectionPercentage,
        double expectedContribution,
        double expectedSettlementBasis)
    {
        var actual = CreateDocument(
            Berechnungsart.Haushaltssumme,
            insuranceSum,
            risk,
            hasWebshop,
            additionalProtectionRequested,
            additionalProtectionPercentage);

        VerifyCorrectlyCalculated(expectedContribution, expectedSettlementBasis, actual);
    }

    private static void VerifyCorrectlyCalculated(
        double expectedContribution,
        double expectedSettlementBasis,
        Dokument actual)
    {
        actual.Kalkuliere();
        actual.Beitrag.Should().Be((decimal)expectedContribution);
        actual.Berechnungbasis.Should().Be((decimal)expectedSettlementBasis);
    }

    [Theory]
    [InlineData(InsuranceSum100k,Risiko.Gering, true, true, AdditionalProtectionPercentage20, 46.95, 17.78)]
    [InlineData(InsuranceSum100k,Risiko.Gering, false, true, AdditionalProtectionPercentage20, 23.47, 17.78)]
    [InlineData(InsuranceSum100k,Risiko.Gering, true, false, AdditionalProtectionPercentage20, 39.12, 17.78)]
    [InlineData(9990000,Risiko.Gering, true, true, AdditionalProtectionPercentage25, 154.61, 56.22)]
    public void Kalkuliere_BerechnungsartUmsatz_ReturnsCorrectlyCalculated(
        double insuranceSum,
        Risiko risk,
        bool hasWebshop,
        bool additionalProtectionRequested,
        float additionalProtectionPercentage,
        double expectedContribution,
        double expectedSettlementBasis)
    {
        var actual = CreateDocument(
            Berechnungsart.Umsatz,
            insuranceSum,
            risk,
            hasWebshop,
            additionalProtectionRequested,
            additionalProtectionPercentage);

        VerifyCorrectlyCalculated(expectedContribution, expectedSettlementBasis, actual);
    }
    
    
    [Theory]
    [InlineData(InsuranceSum100k,Risiko.Mittel, true, true, AdditionalProtectionPercentage20, 31200, 100)]
    [InlineData(InsuranceSum5M,Risiko.Gering, true, true, AdditionalProtectionPercentage25, 1250000, 5000)]
    [InlineData(InsuranceSum5M,Risiko.Gering, true, false, 0, 1000000, 5000)]
    [InlineData(InsuranceSum5M,Risiko.Gering, false, true, AdditionalProtectionPercentage20, 1200000, 5000)]
    [InlineData(InsuranceSum200k,Risiko.Gering, true, true, AdditionalProtectionPercentage25, 50000, 200)]
    public void Kalkuliere_BerechnungsartAnzahlMitarbeiter_ReturnsCorrectlyCalculated(
        double insuranceSum,
        Risiko risk,
        bool hasWebshop,
        bool additionalProtectionRequested,
        float additionalProtectionPercentage,
        double expectedContribution,
        double expectedSettlementBasis)
    {
        var actual = CreateDocument(
            Berechnungsart.AnzahlMitarbeiter,
            insuranceSum,
            risk,
            hasWebshop,
            additionalProtectionRequested,
            additionalProtectionPercentage);

        VerifyCorrectlyCalculated(expectedContribution, expectedSettlementBasis, actual);
    }

    private static Dokument CreateDocument(
        Berechnungsart calculationType,
        double insuranceSum,
        Risiko risk,
        bool hasWebshop,
        bool additionalProtectionRequested,
        float? additionalProtectionPercentage = null)
    {
        var percentage = additionalProtectionPercentage ?? 0;
        var actual = new Dokument(FixedDocumentId)
        {
            Berechnungsart = calculationType,
            Risiko = risk,
            HatWebshop = hasWebshop,
            Typ = DocType,
            Versicherungssumme = (decimal)insuranceSum,
            InkludiereZusatzschutz = additionalProtectionRequested,
            ZusatzschutzAufschlag = percentage,
            VersicherungsscheinAusgestellt = IsInsurancePolicyIssued,
        };
        return actual;
    }
}