using System.Reflection;
using Faker.Generator;
using Faker.Generator.impl;

namespace Faker;

public class Faker
{
    public T Create<T>()
    {
        var myType = typeof(T);

        if (!myType.IsPrimitive)
        {
            T obj = (T) Activator.CreateInstance(typeof(T), new object[] { });

            try
            {
                //Fields
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
                            field.SetValue(obj, ReturnValue(field.FieldType));
                        }
                    }
                }

                //Properties
                foreach (var property in myType.GetProperties())
                {
                    if (property.GetSetMethod() != null)
                    {
                        if (property.PropertyType == typeof(List<T>))
                        {
                            var listGenerator = new ListGenerator();
                            property.SetValue(obj, listGenerator.Generate<T>(this));
                        }
                        else
                        {
                            property.SetValue(obj, ReturnValue(property.PropertyType));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : {0} ", e.StackTrace);
            }

            return obj;
        }

        return (T) Activator.CreateInstance(typeof(T));
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
            throw new Exception("No such type");
        }

        return generator.Generate();
    }
}