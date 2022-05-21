namespace FakeStorage
{
    public class FakeStorageSettings
    {
        public Range MillisecondsOfWaitForDelete { get; set; }
        public Range MillisecondsOfWaitForInsert { get; set; }
        public Range MillisecondsOfWaitForUpdate { get; set; }
        public Range MillisecondsOfWaitForGet { get; set; }
        public Range MillisecondsOfWaitForWhere { get; set; }
        public List<ExceptionOdds> ExceptionOddsForDelete { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForInsert { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForUpdate { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForGet { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForWhere { get; } = new();
    }
}