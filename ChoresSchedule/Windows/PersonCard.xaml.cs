using ChoresSchedule.Classes;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChoresSchedule.Windows
{
    /// <summary>
    /// Логика взаимодействия для PersonCard.xaml
    /// </summary>
    public partial class PersonCard : Window
    {
        public PersonCard(Person person)
        {
            InitializeComponent();
            FillingOutAPersonCard(person);
        }

        private void FillingOutAPersonCard(Person person)
        {
            var sick = person.IsSick ? "Да" : "Нет";
            string details = $"Имя: {person.PersonName}\n" +
                             $"Пол: {person.Gender}\n" +
                             $"Тип: {person.PersonAge}\n" +
                             $"Степень занятости: {person.EmploymentRate}\n" +
                             $"Положение: {person.FamilySituation}\n" +
                             $"Болен: {sick}";

            informationTBl.Text = details;
            userImage.Source = new BitmapImage(new Uri(person.PathImage, UriKind.Relative));
        }
    }
}
