using FakeStorage.Dummy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FakeStorage
{
    public class FakeStorageBuilder<T, TKey>
    {
        private readonly IServiceCollection _services;

        public FakeStorageBuilder(IServiceCollection services)
        {
            _services = services;
        }
        public FakeStorageBuilder<T, TKey> AddFakeStorage(Action<FakeStorageSettings> settings)
            => _services!.AddFakeStorage<T, TKey>(settings);
        public FakeStorageBuilder<T, TKey> AddRandomData(Expression<Func<T, TKey>> navigationKey, int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
        {
            var nameOfKey = navigationKey.ToString().Split('.').Last();
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < numberOfElements; i++)
            {
                var entity = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    property.SetValue(entity, Creator.Transform(property, numberOfElementsWhenEnumerableIsFound));
                }
                var key = properties.First(x => x.Name == nameOfKey).GetValue(entity);
                Storage<T, TKey>._values.Add((TKey)key!, entity);
            }
            return this;
        }
        public IServiceCollection Finalize() 
            => _services!;
    }
}
