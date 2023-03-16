using System.Net;
using System.Text;
using System.Text.Json;
using CodeSpire.Api.Features.AddDocument;
using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Api.Test.Features.AddDocumentTests;

public sealed class MitUngueltigerAnfrage : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _webApplicationFactory;

    public MitUngueltigerAnfrage(TestWebApplicationFactory webApplicationFactory) => _webApplicationFactory = webApplicationFactory;

    [Theory]
    [MemberData(nameof(GeneriereUngueligeAnfragen))]
    internal async Task GibtBadRequestMitStatusCode400Zurueck(AddDocumentRequest request)
    {
        // Arrange.
        var httpClient = _webApplicationFactory.CreateClient();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // Act.
        var response = await httpClient.PostAsync("/documents", content);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    public static IEnumerable<object[]> GeneriereUngueligeAnfragen()
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
                ZusatzschutzAufschlag = 0
            }
        };
    }
}