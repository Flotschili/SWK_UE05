using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonAdmin.Domain
{
    public class Person
    {
        public Person(int id, string firstName, string lastName, DateTime dateofBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateofBirth = dateofBirth;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateofBirth { get; set; }


        public override string ToString() =>
            $"{FirstName} {LastName} ({Id}): {DateofBirth:yyy.MM.dd}";
    }
}
