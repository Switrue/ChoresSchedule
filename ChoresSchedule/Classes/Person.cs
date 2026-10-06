using ChoresSchedule.Classes.Data_Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChoresSchedule.Classes
{
    public class Person
    {
        public string PersonName { get; set; }
        public string PersonAge { get; set; }
        public string PathImage { get; set; }
        public string Gender { get; set; }
        public string EmploymentRate { get; set; }
        public string FamilySituation { get; set; }
        public bool IsSick {  get; set; }
        public int Id { get; set; }

        private static Dictionary<int, Person> persons = new Dictionary<int, Person>();

        public Person(int id, string name, string age, string pathImage, string gender, string employmentRate, string familySituation, bool isSick)
        {
            Id = id;
            PersonName = name;
            PersonAge = age;
            PathImage = pathImage;
            Gender = gender;
            EmploymentRate = employmentRate;
            FamilySituation = familySituation;
            IsSick = isSick;

            AddPerson(this);
        }

        public double PriorityFactor()
        {
            var sick = IsSick ? 1.5 : 1;
            var age = PersonAge == Age.Adult.ToString() ? 0.5 : 1;

            double result = Data.ListOfOccupationalStatus[EmploymentRate] * Data.ListOfFamilySituations[FamilySituation] / sick * age;

            return Math.Round(result, 2);
        }

        public static void AddPerson(Person person)
        {
            persons[person.Id] = person;
        }

        public static Person GetPerson(int id)
        {
            persons.TryGetValue(id, out Person person);
            return person;
        }

        public static bool RemovePerson(int id)
        {
            return persons.Remove(id);
        }

        public static List<Person> GetAllPersonsList()
        {
            return persons.Values.ToList();
        }

        public static IEnumerable<Person> GetAllPersons()
        {
            return persons.Values.ToList();
        }

        public override string ToString()
        {
            return $"Name: {PersonName}, Age: {PersonAge}, Gender: {Gender}, Employment Rate: {EmploymentRate}, Family Situation: {FamilySituation}, Is Sick: {IsSick}";
        }
    }
}
