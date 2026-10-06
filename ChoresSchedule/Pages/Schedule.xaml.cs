using ChoresSchedule.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ChoresSchedule.Windows
{
    /// <summary>
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Page
    {

        public Schedule()
        {
            InitializeComponent();
            DefaultParams();
            LoadCalendar(DateTime.Now);
        }

        private void DefaultParams()
        {
            infoBtn.Visibility = Data.adminMode ? Visibility.Visible : Visibility.Collapsed;
        }

        #region LoadCalendar

        private void LoadCalendar(DateTime date)
        {
            MonthTextBlock.Text = date.ToString("MMMM yyyy");

            // Получаем первый день месяца и количество дней в месяце
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startDay = (int)firstDayOfMonth.DayOfWeek;

            // Заполняем календарь пустыми ячейками до первого дня месяца
            for (int i = 0; i < startDay; i++)
            {
                CalendarGrid.Children.Add(new TextBlock());
            }

            // Получаем всех персон
            var persons = Person.GetAllPersons().ToList();
            var daysAllocation = AllocateDays(persons, daysInMonth);
            FillCalendar(daysAllocation, persons, daysInMonth, startDay);
        }

        private Dictionary<Person, int> AllocateDays(List<Person> persons, int daysInMonth)
        {
            var daysAllocation = new Dictionary<Person, int>();
            double totalPriority = persons.Sum(p => p.PriorityFactor());

            // Сначала выделяем дни без округления
            foreach (var person in persons)
            {
                int daysForPerson = (int)((person.PriorityFactor() / totalPriority) * daysInMonth);
                daysAllocation[person] = daysForPerson;
            }

            // Подсчитываем общее количество выделенных дней
            int totalAllocatedDays = daysAllocation.Values.Sum();

            // Если общее количество выделенных дней меньше, чем daysInMonth, распределяем оставшиеся дни
            int remainingDays = daysInMonth - totalAllocatedDays;

            // Сортируем персон по убыванию приоритета
            var sortedPersons = persons.OrderByDescending(p => p.PriorityFactor()).ToList();

            // Распределяем оставшиеся дни
            for (int i = 0; i < remainingDays; i++)
            {
                foreach (var person in sortedPersons)
                {
                    // Условие для добавления дня, чтобы не превышать среднее количество дней
                    if (daysAllocation[person] < (daysInMonth / persons.Count) + (remainingDays > 0 ? 1 : 0))
                    {
                        daysAllocation[person]++;
                        break;
                    }
                }
            }

            return daysAllocation;
        }

        private void FillCalendar(Dictionary<Person, int> daysAllocation, List<Person> persons, int daysInMonth, int startDay)
        {
            var random = new Random();

            for (int currentDay = 1; currentDay <= daysInMonth; currentDay++)
            {
                var availablePersons = persons.Where(p => daysAllocation[p] > 0).ToList();
                if (!availablePersons.Any()) break;

                // Выбираем случайного человека на основе оставшихся
                var selectedPerson = availablePersons[random.Next(availablePersons.Count)];

                // Создаем текстблок для дня
                var textBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = (Double)FindResource("DefaultFontSize"),
                    TextAlignment = TextAlignment.Center,
                    Text = $"{currentDay}\n{selectedPerson.PersonName}"
                };

                textBlock.MouseDown += (s, e) => { new PersonCard(selectedPerson).ShowDialog(); };

                var border = new Border
                {
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(3),
                    BorderBrush = Brushes.Transparent,
                    Child = textBlock
                };

                // Установка обработчиков событий для изменения цвета границы
                var borderBrush = new SolidColorBrush(Colors.Transparent);
                border.BorderBrush = borderBrush;

                border.MouseEnter += (s, e) => { borderBrush.Color = (Color)ColorConverter.ConvertFromString("#bee6fd"); }; 
                border.MouseLeave += (s, e) => { borderBrush.Color = Colors.Transparent; };

                // Вставляем Border в календарь на нужную позицию
                int positionIndex = currentDay + startDay - 1;
                CalendarGrid.Children.Insert(positionIndex, border);

                // Уменьшаем количество дней, оставшихся для этого экземпляра
                daysAllocation[selectedPerson]--;
            }
        }

        #endregion

        private void NavigateToQuestionnaire(object sender, RoutedEventArgs e)
        {
            Questionnaire questionnaire = new Questionnaire();
            var allPersons = Person.GetAllPersons();

            foreach (var person in allPersons)
            {
                if (IsValidImagePath(person.PathImage))
                {
                    var card = CreatePersonCard(questionnaire, person);
                    card.Tag = person.Id;
                    questionnaire.cardsContainer.Children.Add(card);
                }
                else
                    Debug.WriteLine("Некорректный относительный URI для изображения: " + person.PathImage);
            }

            NavigationService.Navigate(questionnaire);
        }

        private bool IsValidImagePath(string pathImage)
        {
            return Uri.IsWellFormedUriString(pathImage, UriKind.Relative);
        }

        private Border CreatePersonCard(Questionnaire questionnaire, Person person)
        {
            return questionnaire.CreateCard(
                person.PathImage,
                person.PersonName,
                person.Gender,
                person.PersonAge,
                person.EmploymentRate,
                person.FamilySituation,
                person.IsSick
            );
        }

        private void PersonInfo_Click(object sender, RoutedEventArgs e)
        {
            Methods.MessageAllPersons();
        }
    }
}
