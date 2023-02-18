using CodeSpire.Domain;
using CodeSpire.Domain.Interfaces;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Features;

/// <summary>
/// Response containing all available documents.
/// </summary>
internal sealed record ListDocumentsResponse
{
    public IReadOnlyCollection<Dokument> Documents { get; init; } = new List<Dokument>();
}

// ReSharper disable once UnusedType.Global
internal sealed class ListDocumentsEndpoint : EndpointWithoutRequest<ListDocumentsResponse>
{
    private readonly IRepository _repo;

    public ListDocumentsEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Get("/documents");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces<ListDocumentsResponse>(200, "application/json+custom");
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var documents = _repo.List();
        Response = new()
        {
            Documents = documents.ToList()
        };
        return Task.CompletedTask;
    }
}