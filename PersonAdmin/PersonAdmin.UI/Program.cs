using Dal.Common;
using Microsoft.Extensions.Configuration;
using PersonAdmin.BusinessLogic;
using PersonAdmin.Dal.Ado;
using PersonAdmin.Dal.Interface;
using PersonAdmin.Dal.Simple;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "PersonDbConnection");

await TestAsync(new SimplePersonDao());
await TestAsync(new AdoPersonDao(connectionFactory));

async Task TestAsync(IPersonDao personDao)
{
    Console.WriteLine(personDao.GetType());

    var tester = new DalTester(personDao);

    await tester.TestFindAllAsync();
    await tester.TestFindByIdAsync();
    await tester.TestUpdateAsync();
    await tester.TestTransactionsAsync();
    await tester.TestInsertAsync();

    Console.WriteLine();
}