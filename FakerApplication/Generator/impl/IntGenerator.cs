namespace Faker.Generator.impl;

class IntGenerator : IGenerator
{
    public override object Generate()
    {
        return random.Next();
    }
}