using Microsoft.Extensions.DependencyInjection;

namespace FakeStorage
{
    public static class ServiceCollectionExtensions
    {
        //add the part of dummy random data
        public static FakeStorageBuilder<T, TKey> AddFakeStorage<T, TKey>(this IServiceCollection services, Action<FakeStorageSettings> settings)
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
            return new FakeStorageBuilder<T, TKey>(services);

            static void Check(List<ExceptionOdds> odds)
            {
                var total = odds.Sum(x => x.Percentage);
                if (odds.Where(x => x.Percentage <= 0 || x.Percentage > 100).Any())
                {
                    throw new ArgumentException("Some percentages are wrong, greater than 100% or lesser than 0.");
                }
                if (total > 100)
                    throw new ArgumentException("Your total percentage is greater than 100.");
            }
        }
    }
}