using System.Text;
using CodeSpire.Application;
using CodeSpire.Domain;
using Newtonsoft.Json;

namespace CodeSpire.Infrastructure;

public class JsonRepository : IRepository
{
    private readonly string _file;
    private Dictionary<Guid,Dokument> _dokumente = new();

    public JsonRepository(string file)
    {
        _file = file;
        Load();
    }

    private void Load()
    {
        if (!File.Exists(_file))
        {
            var empty = new Dictionary<Guid, Dokument>();
            File.WriteAllText(_file, JsonConvert.SerializeObject(empty), new UTF8Encoding());
        }

        var json = File.ReadAllText(_file, Encoding.UTF8);
        _dokumente = JsonConvert.DeserializeObject<Dictionary<Guid,Dokument>>(json) ?? new Dictionary<Guid, Dokument>();
    }

    public Dokument? Find(Guid id)
    {
        return _dokumente.TryGetValue(id, out var document)
            ? document
            : null;
    }

    public IEnumerable<Dokument> List()
    {
        return _dokumente.Values;
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
        var id = Guid.NewGuid();
        var dict = new Dictionary<Guid, Dokument>();
        dict.Add(id, new Dokument(id));
        var json = JsonConvert.SerializeObject(dict, Formatting.Indented);
        File.WriteAllText(_file, json, new UTF8Encoding());
    }
}