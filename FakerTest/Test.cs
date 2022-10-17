using NUnit.Framework;

namespace FakerTest;

using Faker;

public class Test
{
    private Faker _faker;

    [SetUp]
    public void SetupBeforeEachTest()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void TestSimpleClass()
    {
        var simpleClass = _faker.Create<SimpleClass>();
        
        Assert.NotZero(simpleClass.i);
        Assert.NotZero(simpleClass.d);
        Assert.NotZero(simpleClass.IWithPublicSet);
        Assert.Zero(simpleClass.IWithPrivateSet);
    }
    
    [Test]
    public void TestCycleDependency()
    {
        _faker.Create<A>();
        _faker.Create<B>();
        _faker.Create<C>();
    }
    
    [Test]
    public void TestDiversityClass()
    {
        var diversityClass = _faker.Create<DiversityClass>();
        
        Assert.IsNotNull(diversityClass.simpleClass);
        Assert.NotZero(diversityClass.simpleClass.i);
        Assert.NotZero(diversityClass.simpleClass.d);
        
        Assert.NotZero(diversityClass.i);
        Assert.NotZero(diversityClass.d);
        Assert.NotZero(diversityClass.f);
        Assert.NotZero(diversityClass.l);
        Assert.IsNotEmpty(diversityClass.s);
        Assert.IsNotNull(diversityClass.date);
        Assert.IsNotNull(diversityClass.foos);
        Assert.IsNotNull(diversityClass.Foos);
        Assert.Zero(diversityClass.GetIPr());
    }
    
    [Test]
    public void TestClassWithSeveralConstructors()
    {
        var classWithSeveralConstructors = _faker.Create<ClassWithSeveralConstructors>();

        Assert.NotZero(classWithSeveralConstructors.GetI());
        Assert.NotZero(classWithSeveralConstructors.GetD());
    }
    
    [Test]
    public void TestClassWithPrivateConstructor()
    {
        _faker.Create<ClassWithPrivateConstructor>();
    }
}