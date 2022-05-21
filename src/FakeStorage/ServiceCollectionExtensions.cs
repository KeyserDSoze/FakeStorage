using Microsoft.Extensions.DependencyInjection;

namespace FakeStorage
{
    public static class ServiceCollectionExtensions
    {
        //add the part of dummy random data
        //adding the check, the total values of odds and the minimum value
        public static IServiceCollection AddFakeStorage<T, TKey>(this IServiceCollection services, Action<FakeStorageSettings> settings)
        {
            var options = new FakeStorageSettings();
            settings.Invoke(options);
            Check(options.ExceptionOddsForWhere);
            Check(options.ExceptionOddsForInsert);
            Check(options.ExceptionOddsForUpdate);
            Check(options.ExceptionOddsForGet);
            Check(options.ExceptionOddsForDelete);
            FakeStorageSettingsFactory.Instance.Settings.Add(nameof(IFakeStorage<T, TKey>), options);
            services.AddSingleton(FakeStorageSettingsFactory.Instance);
            services.AddSingleton<IFakeStorage<T, TKey>, Storage<T, TKey>>();
            return services;
            void Check(List<ExceptionOdds> odds)
            {
                var total = odds.Sum(x => x.Percentage);
                if (odds.Where(x => x.Percentage <= 0 || x.Percentage > 100).Any())
                {
                    throw new Exception();
                }
                if (total > 0 && total <= 100)
                    throw new Exception();
            }
        }
    }
}