using System.Windows;

namespace ChoresSchedule.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        public MessageBoxCustom(string message, string name)
        {
            InitializeComponent();
            MessageText.Text = message;
            Title = name;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }

    public static class CustomMessageBoxHelper
    {
        public static bool Show(string message, string name)
        {
            var messageBox = new MessageBoxCustom(message, name);
            return messageBox.ShowDialog() ?? false;
        }
    }
}
