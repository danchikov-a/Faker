namespace Faker.Generator;

public abstract class IGenerator
{ 
    protected Random random = new(DateTime.Now.Millisecond);
    public abstract object Generate();
}