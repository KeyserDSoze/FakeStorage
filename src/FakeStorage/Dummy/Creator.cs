using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FakeStorage.Dummy
{
    internal static class Creator
    {
        public static dynamic Transform(Type type, string name = null)
        {
            if (type == typeof(int) || type == typeof(int?))
            {
                return new Random().Next(int.MinValue, int.MaxValue);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                return new Random().NextInt64(long.MinValue, long.MaxValue);
            }
            else if (type == typeof(uint) || type == typeof(uint?))
            {
                return new Random().NextInt64(uint.MinValue, uint.MaxValue);
            }
            else if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return new Random().NextInt64(0, long.MaxValue);
            }
            else if (type == typeof(short) || type == typeof(short?))
            {
                return new Random().NextInt64(short.MinValue, short.MaxValue);
            }
            else if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return new Random().NextInt64(ushort.MinValue, ushort.MaxValue);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return (float)(new Random().NextDouble() * float.MaxValue + float.MinValue);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                return new Random().NextDouble() * double.MaxValue + double.MinValue;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return (decimal)new Random().NextDouble() * decimal.MaxValue + decimal.MinValue;
            }
            else if (type == typeof(string))
            {
                return $"{name}_{Guid.NewGuid()}";
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                return new Random().NextDouble() > 0.5D;
            }
            else if (type == typeof(char) || type == typeof(char?))
            {
                return (char)new Random().Next(0, 255);
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                return Guid.NewGuid();
            }
            else if (type == typeof(byte) || type == typeof(byte?))
            {
                return RandomNumberGenerator.GetBytes(1)[0];
            }
            else if (type == typeof(sbyte) || type == typeof(sbyte?))
            {
                return (sbyte)new Random().Next(0, 255);
            }
            else if (!type.IsArray && !type.IsInterface && !type.IsAbstract)
            {
                var entity = Activator.CreateInstance(type)!;
                if (entity is IDictionary)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var newKey = Creator.Transform(type.GetGenericArguments().First(), "Key");
                        var newValue = Creator.Transform(type.GetGenericArguments().Last(), "Value");
                        ((dynamic)entity).Add(newKey, newValue);
                    }
                }
                else if (entity is IEnumerable)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var newValue = Creator.Transform(type.GetGenericArguments().First());
                        ((dynamic)entity).Add(newValue);
                    }
                }
                else
                {
                    try
                    {
                        var properties = type.GetProperties();
                        foreach (var property in properties)
                        {
                            property.SetValue(entity, Creator.Transform(property));
                        }
                    }
                    catch
                    {
                    }
                }
                return entity;
            }
            else
                return default;
        }
        public static dynamic Transform(PropertyInfo propertyInfo)
        {
            Type type = propertyInfo.PropertyType;
            return Transform(type, propertyInfo.Name);
        }
    }
}