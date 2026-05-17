using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Prototype;

public class TemplateRegistry
{
    private readonly Dictionary<string, DocumentTemplate> _prototypes = new();

    public void Register(string key, DocumentTemplate prototype)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Cheia sablonului este obligatorie.", nameof(key));

        _prototypes[key] = prototype;
    }

    public DocumentTemplate GetClone(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"Sablonul '{key}' nu este inregistrat.");

        return prototype.DeepClone();
    }

    public bool Contains(string key) => _prototypes.ContainsKey(key);
}
