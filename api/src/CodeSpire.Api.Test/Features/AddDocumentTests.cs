using System.Net;
using System.Text;
using System.Text.Json;
using CodeSpire.Api.Features;
using CodeSpire.Api.Features.AddDocument;
using CodeSpire.Domain.Models;
using FluentAssertions;

namespace CodeSpire.Api.Test.Features;

public class AddDocumentTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _webApplicationFactory;

    public AddDocumentTests(TestWebApplicationFactory webApplicationFactory) => _webApplicationFactory = webApplicationFactory;
    
    protected HttpClient CreateHttpClient() => _webApplicationFactory.CreateClient();

    [Theory]
    [MemberData(nameof(ValidAddDocumentRequests))]
    internal async Task AddDocument_ValidRequest_ReturnsSuccessful(AddDocumentRequest request)
    {
        // Arrange.
        var httpClient = CreateHttpClient();
        
        // Act.
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/documents", content);

        // Assert.
        response.EnsureSuccessStatusCode();
    }
    
    [Theory]
    [MemberData(nameof(InvalidAddDocumentRequests))]
    internal async Task AddDocument_InvalidValidRequest_ReturnsBadRequest(AddDocumentRequest request)
    {
        // Arrange.
        var httpClient = CreateHttpClient();
        
        // Act.
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/documents", content);

        // Assert.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    public static IEnumerable<object[]> ValidAddDocumentRequests()
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

    public static IEnumerable<object[]> InvalidAddDocumentRequests()
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