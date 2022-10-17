namespace Faker.Generator.impl;

class LongGenerator : IGenerator
{
    public override object Generate()
    {
        return random.NextInt64();
    }
}