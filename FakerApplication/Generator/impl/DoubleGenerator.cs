namespace Faker.Generator.impl;

class DoubleGenerator : IGenerator
{
    public override object Generate()
    {
        return random.NextDouble();
    }
}