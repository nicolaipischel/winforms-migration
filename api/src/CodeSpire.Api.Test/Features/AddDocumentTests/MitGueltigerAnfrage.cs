using System.Text;
using System.Text.Json;
using CodeSpire.Api.Features.AddDocument;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Test.Features.AddDocumentTests;

public sealed class MitGueltigerAnfrage : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _webApplicationFactory;

    public MitGueltigerAnfrage(TestWebApplicationFactory webApplicationFactory) => _webApplicationFactory = webApplicationFactory;

    [Theory]
    [MemberData(nameof(GeneriereGueltigeAnfragen))]
    internal async Task GibtErfolgreichDieGuidDesNeuenDokumentsZuerueck(AddDocumentRequest request)
    {
        // Arrange.
        var httpClient = _webApplicationFactory.CreateClient();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // Act.
        var response = await httpClient.PostAsync("/documents", content);

        // Assert.
        response.EnsureSuccessStatusCode();
    }
    
    public static IEnumerable<object[]> GeneriereGueltigeAnfragen()
    {
        yield return new object[]
        {
            new AddDocumentRequest
            {
                Versicherungssumme = 100000m,
                Berechnungsart = Berechnungsart.Haushaltssumme,
                Risiko = Risiko.Mittel,
                HatWebshop = true,
                InkludiereZusatzschutz = true,
                ZusatzschutzAufschlag = 20f
            }
        };
    }
}