using CodeSpire.Domain;

namespace CodeSpire.Application;

public interface IRepository
{
    Dokument? Find(Guid id);
    List<Dokument> List();
    void Add(Dokument dokument);
    void Save();
}