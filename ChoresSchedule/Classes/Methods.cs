using System.Collections.Generic;
using System.Windows;

namespace ChoresSchedule.Classes
{
    public static class Methods
    {
        public static void MessageAllPersons()
        {
            List<Person> allPersons = Person.GetAllPersonsList();

            foreach (var person in allPersons)
            {
                var coef = person.PriorityFactor();
                MessageBox.Show($"{person.ToString()} | {coef}", $"ID: {person.Id}");
            }
        }

        public static void DeleteAllPersons()
        {
            List<Person> allPersons = Person.GetAllPersonsList();

            foreach (var person in allPersons)
            {
                Person.RemovePerson(person.Id);
            }
        }
    }
}
