// See https://aka.ms/new-console-template for more information
using FakeStorage;
using FakeStorageApp;
using Rystem;

Console.WriteLine("Hello, World!");
double x = 0.4389204324;
Console.WriteLine(x);

ServiceLocator
    .Create()
    .AddFakeStorage<Solomon, string>(options =>
    {
        var customRange = new Range(1000, 2000);
        options.MillisecondsOfWaitForDelete = customRange;
        options.MillisecondsOfWaitForInsert = customRange;
        options.MillisecondsOfWaitForUpdate = customRange;
        options.MillisecondsOfWaitForGet = customRange;
        options.MillisecondsOfWaitForWhere = new Range(3000, 7000);
        var customExceptions = new List<ExceptionOdds>
        {
            new ExceptionOdds()
            {
                Exception = new Exception(),
                Percentage = 0.45
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Salsiccia"),
                Percentage = 0.1
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Salsiccia Gaudia"),
                Percentage = 0.548
            }
        };
        options.ExceptionOddsForDelete.AddRange(customExceptions);
        options.ExceptionOddsForGet.AddRange(customExceptions);
        options.ExceptionOddsForInsert.AddRange(customExceptions);
        options.ExceptionOddsForUpdate.AddRange(customExceptions);
        options.ExceptionOddsForWhere.AddRange(customExceptions);
    })
    .AddRandomData(x => x.Key, 20)
    .Services
    .FinalizeWithoutDependencyInjection();


var storage = ServiceLocator.GetService<IFakeStorage<Solomon, string>>();
await storage.InsertAsync("aaa", new());
await storage.UpdateAsync("aaa", new());
var q = await storage.GetAsync("aaa");
await storage.DeleteAsync("aaa");
var all = await storage.WhereAsync();
var olaf = string.Empty;