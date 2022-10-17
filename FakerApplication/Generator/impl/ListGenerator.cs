namespace Faker.Generator.impl;

class ListGenerator
{
    private Random random = new(DateTime.Now.Millisecond);
    
    public object Generate<T>(Faker faker)
    {
        var list = new List<T>();
        
        int listSize = random.Next(1, 5);
        
        for (int i = 0; i < listSize; i++)
        {
            list.Add(faker.Create<T>());
        }
        
        return list;
    }
}