using Dal.Common;
using Microsoft.Data.SqlClient;
using PersonAdmin.Dal.Interface;
using PersonAdmin.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonAdmin.Dal.Ado
{
    public class AdoPersonDao : IPersonDao
    {
        private readonly AdoTemplate template;

        public AdoPersonDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private Person MapRowToPerson(IDataRecord row)
        {
            return new Person(
                    (int)row["id"],
                    (string)row["first_name"],
                    (string)row["last_name"],
                    (DateTime)row["date_of_birth"]);
        }

        public async Task<IEnumerable<Person>> FindAllAsync()
            => await template.QueryAsync("select * from person", MapRowToPerson);

        public async Task<Person?> FindByIdAsync(int id)
        {
            var result = await template.QueryAsync("select * from person where id = @id",
                    MapRowToPerson,
                    new QueryParameter("@id", id));
            return result.SingleOrDefault();
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            return (await template.ExecuteAsync("update person set first_name = @fn, last_name = @ln, date_of_birth = @dob where id = @id",
                new QueryParameter("@id", person.Id),
                new QueryParameter("@fn", person.FirstName),
                new QueryParameter("@ln", person.LastName),
                new QueryParameter("@dob", person.DateofBirth))) == 1;
        }

        public async Task InsertAsync(Person person)
        {
            var id = await template.ExecuteScalarAsync<decimal>(
                "insert into person (first_name, last_name, date_of_birth) values (@fn, @ln, @dob); select scope_identity()",
                new QueryParameter("@fn", person.FirstName),
                new QueryParameter("@ln", person.LastName),
                new QueryParameter("@dob", person.DateofBirth));

            person.Id = (int)id;
        }
    }
}
