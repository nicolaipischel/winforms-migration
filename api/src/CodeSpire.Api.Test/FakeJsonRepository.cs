using CodeSpire.Domain.Interfaces;
using CodeSpire.Domain.Models;

namespace CodeSpire.Api.Test;

internal sealed class FakeJsonRepository : IRepository
{
    private Dictionary<Guid, Dokument> _dokumente;

    public FakeJsonRepository()
    {
        _dokumente = new Dictionary<Guid, Dokument>();
    }
    public IEnumerable<Dokument> List()
    {
        return _dokumente.Values;
    }

    public Dokument? Find(Guid id)
    {
        return _dokumente.TryGetValue(id, out var document)
            ? document
            : null;
    }

    public void Add(Dokument dokument)
    {
        _dokumente.Add(dokument.Id,dokument);
    }

    public void Update(Dokument dokument)
    {
        _dokumente[dokument.Id] = dokument;
    }

    public void Save()
    {
    }
}