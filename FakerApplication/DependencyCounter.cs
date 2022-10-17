namespace Faker;

public class DependencyCounter
{
    private readonly Dictionary<Type, int> _dependencyCounter = new();

    public void AddType(Type type)
    {
        if (_dependencyCounter.ContainsKey(type))
        {
            _dependencyCounter[type]++;
        }
        else
        {
            _dependencyCounter.Add(type, 1);
        }
    }

    public void DeleteType(Type type)
    {
        if (_dependencyCounter.ContainsKey(type) && _dependencyCounter[type] > 1)
        {
            _dependencyCounter[type]--;
        }
        else
        {
            _dependencyCounter.Remove(type);
        }
    }

    public bool IsCycled()
    {
        return _dependencyCounter.Values.Any(typeCount => typeCount > 2);
    }
}