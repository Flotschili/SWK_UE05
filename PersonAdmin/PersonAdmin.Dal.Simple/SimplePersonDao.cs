using PersonAdmin.Dal.Interface;
using PersonAdmin.Domain;
using System.Threading.Tasks;

namespace PersonAdmin.Dal.Simple;

public class SimplePersonDao : IPersonDao
{
    private static IList<Person> personList = new List<Person>
    {
        new Person(1, "John", "Doe", DateTime.Now.AddYears(-10)),
        new Person(2, "Jane", "Doe", DateTime.Now.AddYears(-20)),
        new Person(3, "Max", "Mustermann", DateTime.Now.AddYears(-30))
    };

    public Task<IEnumerable<Person>> FindAllAsync()
    {
        return Task.FromResult<IEnumerable<Person>>(personList); // oder Methode async machen
    }

    public Task<Person?> FindByIdAsync(int id)
    {
        return Task.FromResult<Person?>(personList.SingleOrDefault(p => p.Id == id));
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        var existingPerson = await FindByIdAsync(person.Id);
        if (existingPerson is null)
            return false;

        personList.Remove(existingPerson);
        personList.Add(person);
        return true;
    }

    public Task InsertAsync(Person person)
    {
        person.Id = personList.Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;
        personList.Add(person);

        return Task.CompletedTask;
    }
}
