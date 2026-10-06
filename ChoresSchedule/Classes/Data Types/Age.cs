namespace ChoresSchedule.Classes.Data_Types
{
    internal class Age
    {
        public static readonly Age Adult = new Age("Взрослый");
        public static readonly Age Child = new Age("Ребенок");

        public string Name { get; private set; }

        private Age(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
