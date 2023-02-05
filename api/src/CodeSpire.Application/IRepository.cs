using CodeSpire.Domain;

namespace CodeSpire.Application;

public interface IRepository
{
    IEnumerable<Dokument> List();
    Dokument? Find(Guid id);
    void Add(Dokument dokument);
    void Update(Dokument dokument);
    void Save();
}