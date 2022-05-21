using Microsoft.Extensions.DependencyInjection;

namespace FakeStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFakeStorage<T, TKey>(this IServiceCollection services, Action<FakeStorageSettings> settings)
        {
            var options = new FakeStorageSettings();
            settings.Invoke(options);
            FakeStorageSettingsFactory.Instance.Settings.Add(nameof(IFakeStorage<T, TKey>), options);
            services.AddSingleton<FakeStorageSettingsFactory>(FakeStorageSettingsFactory.Instance);
            services.AddSingleton<IFakeStorage<T, TKey>, Storage<T, TKey>>();
            return services;
        }
    }
}