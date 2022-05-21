namespace FakeStorage
{
    public class FakeStorageSettingsFactory
    {
        public static FakeStorageSettingsFactory Instance { get; } = new();
        public Dictionary<string, FakeStorageSettings> Settings { get; } = new();
        private FakeStorageSettingsFactory() { }
    }
}