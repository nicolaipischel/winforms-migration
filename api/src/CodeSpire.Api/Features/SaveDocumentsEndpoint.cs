using CodeSpire.Domain.Interfaces;

namespace CodeSpire.Api.Features;

// ReSharper disable once UnusedType.Global
internal sealed class SaveDocumentsEndpoint : EndpointWithoutRequest
{
    private readonly IRepository _repo;

    public SaveDocumentsEndpoint(IRepository repo)
    {
        _repo = repo;
    }
    
    public override void Configure()
    {
        Put("/documents");
        AllowAnonymous();
        Description(b =>
        {
            b.Produces(204);
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        _repo.Save();
        return Task.CompletedTask;
    }
}