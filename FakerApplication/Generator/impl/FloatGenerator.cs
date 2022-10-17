namespace Faker.Generator.impl;

class FloatGenerator : IGenerator
{
    public override object Generate()
    {
        return random.NextSingle();
    }
}