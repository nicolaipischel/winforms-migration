using CodeSpire.Domain.Interfaces;

namespace CodeSpire.Api.Features.ListDocument;

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