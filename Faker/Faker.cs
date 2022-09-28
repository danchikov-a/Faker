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
            try
            {
                ConstructorInfo constructor = FindConstructorWithMaxIndex(myType);
                var parameters = constructor.GetParameters();
                var parametersInvokeInConstructor = new object[parameters.Length];

                for (var index = 0; index < parameters.Length; index++)
                {
                    parametersInvokeInConstructor[index] = ReturnValue(parameters[index].ParameterType);
                }

                T obj = (T) Activator.CreateInstance(typeof(T), parametersInvokeInConstructor);

                try
                {
                    //Fields
                    foreach (var field in myType.GetFields())
                    {
                        if (field.IsPublic)
                        {
                            var genericType = field.FieldType.GenericTypeArguments;

                            if (genericType.Length > 0 &&
                                field.FieldType == typeof(List<>).MakeGenericType(genericType))
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

                    //Properties
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
                                    // .net поставит дефолтное значение
                                }
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
            catch (Exception e)
            {
                return default(T);
            }
        }

        return (T) Activator.CreateInstance(typeof(T));
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
            throw new Exception("No such type");
        }

        return generator.Generate();
    }
}