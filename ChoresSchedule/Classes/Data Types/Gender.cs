namespace ChoresSchedule.Classes
{
    internal class Gender
    {
        public static readonly Gender Male = new Gender("Мужской");
        public static readonly Gender Female = new Gender("Женский");

        public string Name { get; private set; }

        private Gender(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
