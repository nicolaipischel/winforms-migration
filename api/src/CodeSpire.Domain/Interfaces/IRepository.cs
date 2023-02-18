using CodeSpire.Domain.Models;

namespace CodeSpire.Domain.Interfaces;

public interface IRepository
{
    IEnumerable<Dokument> List();
    Dokument? Find(Guid id);
    void Add(Dokument dokument);
    void Update(Dokument dokument);
    void Save();
}