namespace FakeStorage
{
    public interface IFakeStorage<T, TKey>
    {
        Task<bool> InsertAsync(TKey key, T value);
        Task<bool> UpdateAsync(TKey key, T value);
        Task<T?> GetAsync(TKey key);
        Task<bool> DeleteAsync(TKey key);
        Task<IEnumerable<T>> WhereAsync(Func<T, bool> predicate);
    }
}