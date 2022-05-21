using System.Security.Cryptography;

namespace FakeStorage
{
    internal class Storage<T, TKey> : IFakeStorage<T, TKey>
    {
        private readonly FakeStorageSettings _settings;

        public Storage(FakeStorageSettingsFactory settings)
        {
            _settings = settings.Settings[nameof(IFakeStorage<T, TKey>)];
        }
        private readonly Dictionary<TKey, T> _values = new();
        private static int GetRandomNumber(Range range)
        {
            int maxPlusOne = range.End.Value + 1 - range.Start.Value;
            return RandomNumberGenerator.GetInt32(maxPlusOne) + range.Start.Value;
        }
        private static Exception? GetException(List<ExceptionOdds> odds)
        {
            if (odds.Count == 0)
                return default;
            var oddBase = (int)Math.Pow(10, odds.Select(x => x.Percentage.ToString()).OrderByDescending(x => x.Length).First().Split('.').Last().Length);
            List<ExceptionOdds> normalizedOdds = new();
            foreach (var odd in odds)
            {
                normalizedOdds.Add(new ExceptionOdds
                {
                    Exception = odd.Exception,
                    Percentage = odd.Percentage * oddBase
                });
            }
            Range range = new(0, 100 * oddBase);
            var result = GetRandomNumber(range);
            int total = 0;
            foreach (var odd in normalizedOdds)
            {
                var value = (int)odd.Percentage;
                if (result >= total && result < total + value)
                    return odd.Exception;
                total += value;
            }
            return default;
        }
        public async Task<bool> DeleteAsync(TKey key)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForDelete));
            var exception = GetException(_settings.ExceptionOddsForDelete);
            if (exception != null)
                throw exception;
            if (_values.ContainsKey(key))
                return _values.Remove(key);
            return false;
        }

        public async Task<T?> GetAsync(TKey key)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForGet));
            var exception = GetException(_settings.ExceptionOddsForGet);
            if (exception != null)
                throw exception;
            return _values.ContainsKey(key) ? _values[key] : default(T);
        }

        public async Task<bool> InsertAsync(TKey key, T value)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForInsert));
            var exception = GetException(_settings.ExceptionOddsForInsert);
            if (exception != null)
                throw exception;
            if (!_values.ContainsKey(key))
            {
                _values.Add(key, value);
                return true;
            }
            else
                return false;
        }
        public async Task<bool> UpdateAsync(TKey key, T value)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForUpdate));
            var exception = GetException(_settings.ExceptionOddsForUpdate);
            if (exception != null)
                throw exception;
            if (_values.ContainsKey(key))
            {
                _values[key] = value;
                return true;
            }
            else
                return false;
        }

        public async Task<IEnumerable<T>> WhereAsync(Func<T, bool> predicate)
        {
            await Task.Delay(GetRandomNumber(_settings.MillisecondsOfWaitForWhere));
            var exception = GetException(_settings.ExceptionOddsForWhere);
            if (exception != null)
                throw exception;
            return _values.Select(x => x.Value).Where(predicate);
        }
    }
}