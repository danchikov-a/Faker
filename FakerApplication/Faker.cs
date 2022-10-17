using System.Reflection;
using Faker.Generator;
using Faker.Generator.impl;

namespace Faker;

public class Faker
{
    private DependencyCounter _dependencyCounter = new ();
    
    public T Create<T>()
    {
        var myType = typeof(T);
        
        _dependencyCounter.AddType(myType);
        
        if(!_dependencyCounter.IsCycled())
        {
            if (!myType.IsPrimitive)
            {
                try
                {
                    T obj = (T) Activator.CreateInstance(typeof(T), CreateObjectWithConstructor(myType));

                    GenerateFields<T>(myType, obj);
                    GenerateProperties<T>(myType, obj);

                    return obj;
                }
                catch
                {
                    return default(T);
                }
            }    
        }
        
        _dependencyCounter.DeleteType(myType);

        return (T) Activator.CreateInstance(typeof(T));
    }

    private void GenerateProperties<T>(Type myType, T obj)
    {
        foreach (var property in myType.GetProperties())
        {
            if (property.GetSetMethod() != null)
            {
                var genericType = property.PropertyType.GenericTypeArguments;

                if (genericType.Length > 0 &&
                    property.PropertyType == typeof(List<>).MakeGenericType(genericType))
                {
                    var listGenerator = new ListGenerator();

                    var method = typeof(ListGenerator).GetMethod("Generate");
                    var generic = method.MakeGenericMethod(genericType);
                    var generatedList = generic.Invoke(listGenerator, new[] {this});

                    property.SetValue(obj, generatedList);
                }
                else
                {
                    try
                    {
                        property.SetValue(obj, ReturnValue(property.PropertyType));
                    }
                    catch (Exception e)
                    {
                        property.SetValue(obj, default);
                    }
                }
            }
        }
    }

    private void GenerateFields<T>(Type myType, T obj)
    {
        foreach (var field in myType.GetFields())
        {
            if (field.IsPublic)
            {
                var genericType = field.FieldType.GenericTypeArguments;

                if (genericType.Length > 0 && field.FieldType == typeof(List<>).MakeGenericType(genericType))
                {
                    var listGenerator = new ListGenerator();

                    var method = typeof(ListGenerator).GetMethod("Generate");
                    var generic = method.MakeGenericMethod(genericType);
                    var generatedList = generic.Invoke(listGenerator, new[] {this});

                    field.SetValue(obj, generatedList);
                }
                else
                {
                    try
                    {
                        field.SetValue(obj, ReturnValue(field.FieldType));
                    }
                    catch (Exception e)
                    {
                        field.SetValue(obj, default);
                    }
                }
            }
        }
    }

    private object[] CreateObjectWithConstructor(Type myType)
    {
        try
        {
            ConstructorInfo constructor = FindConstructorWithMaxIndex(myType);

            var parameters = constructor.GetParameters();
            var parametersInvokeInConstructor = new object[parameters.Length];

            for (var index = 0; index < parameters.Length; index++)
            {
                parametersInvokeInConstructor[index] = ReturnValue(parameters[index].ParameterType);
            }

            return parametersInvokeInConstructor;
        }
        catch
        {
            throw new Exception();
        }
    }

    private ConstructorInfo FindConstructorWithMaxIndex(Type myType)
    {
        if (myType.GetConstructors().Length > 0)
        {
            int constructorWithMaxIndex = 0;
            int max = 0;

            for (int i = 0; i < myType.GetConstructors().Length; i++)
            {
                int paramsSize = myType.GetConstructors()[i].GetParameters().Length;

                if (paramsSize > max)
                {
                    max = paramsSize;
                    constructorWithMaxIndex = i;
                }
            }

            return myType.GetConstructors()[constructorWithMaxIndex];
        }

        throw new Exception();
    }

    private object ReturnValue(Type type)
    {
        IGenerator generator;

        if (type == typeof(int))
        {
            generator = new IntGenerator();
        }
        else if (type == typeof(long))
        {
            generator = new LongGenerator();
        }
        else if (type == typeof(float))
        {
            generator = new FloatGenerator();
        }
        else if (type == typeof(double))
        {
            generator = new DoubleGenerator();
        }
        else if (type == typeof(string))
        {
            generator = new StringGenerator();
        }
        else if (type == typeof(DateOnly))
        {
            generator = new DateGenerator();
        }
        else
        {
            var method = typeof(Faker).GetMethod("Create");
            var generic = method.MakeGenericMethod(type);
            var obj = generic.Invoke(this, new object[] {});

            return obj;
        }
        
        return generator.Generate();
    }
}