using ChoresSchedule.Classes;
using ChoresSchedule.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ChoresSchedule.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Questionnaire());
        }

        private void informationBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomMessageBoxHelper.Show(Data.aboutProgram, "О программе");
        }

        private void exitBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangingProgramInformation_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Data.aboutProgram = Data.aboutProgram == Data.aboutProgramInformation ? "Информация удалена..." : Data.aboutProgramInformation;
        }
    }
}
