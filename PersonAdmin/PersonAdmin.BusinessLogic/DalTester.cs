using PersonAdmin.Dal.Interface;
using PersonAdmin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PersonAdmin.BusinessLogic
{
    public class DalTester
    {
        private readonly IPersonDao personDao;

        public DalTester(IPersonDao personDao)
        {
            this.personDao = personDao;
        }

        public async Task TestFindAllAsync()
        {
            Console.WriteLine("FindAll");

            var persons = await this.personDao
                .FindAllAsync();

            persons
                .ToList()
                .ForEach(p => Console.WriteLine(
                    $"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateofBirth,10:yyy-MM-dd}"));
            Console.WriteLine();
        }

        public async Task TestFindByIdAsync()
        {
            Person? person = await personDao.FindByIdAsync(1);
            Console.WriteLine($"FindById(1) -> {person?.ToString() ?? "<null>"}");
            Console.WriteLine();
        }

        public async Task TestUpdateAsync()
        {
            Person? person = await personDao.FindByIdAsync(1);
            Console.WriteLine($"before update: {person}");

            if (person is null)
            {
                return;
            }

            person.DateofBirth = person.DateofBirth.AddYears(-1);
            await personDao.UpdateAsync(person);

            person = await personDao.FindByIdAsync(1);
            Console.WriteLine($"after update: {person}");

            Console.WriteLine();
        }

        public async Task TestTransactionsAsync()
        {
            Console.WriteLine("Test Transaktions");

            try
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await personDao.UpdateAsync(new Person(2, "Before", "Exception", DateTime.Now));

                    //throw new Exception("Something went wrong");

                    await personDao.UpdateAsync(new Person(2, "After", "Exception", DateTime.Now));

                    transaction.Complete(); //nicht completete Transaktionen werden gerollbackt
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(await personDao.FindByIdAsync(2));

            Console.WriteLine();
        }

        public async Task TestInsertAsync()
        {
            var person = new Person(0, "New", "Person", DateTime.Now);
            await personDao.InsertAsync(person);

            person = await personDao.FindByIdAsync(person.Id);
            Console.WriteLine("Inserted: " + person);
            Console.WriteLine();
        }
    }
}
