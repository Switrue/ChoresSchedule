using ChoresSchedule.Classes;
using ChoresSchedule.Classes.Data_Types;
using ChoresSchedule.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ChoresSchedule.Windows
{
    /// <summary>
    /// Логика взаимодействия для Questionnaire.xaml
    /// </summary>
    public partial class Questionnaire : Page
    {
        public static List<string> listOfOccupationalStatus => new List<string>(Data.ListOfOccupationalStatus.Keys);
        public static List<string> listOfFamilySituations => new List<string>(Data.ListOfFamilySituations.Keys);

        private int _tagId;

        public Questionnaire()
        {
            InitializeComponent();
            DefaultParams();
        }

        private void DefaultParams()
        {
            infoBtn.Visibility = Data.adminMode ? Visibility.Visible : Visibility.Collapsed;
            DeveloperModeChB.IsChecked = Data.adminMode;
            DataContext = this;
        }

        private void NavigateToSchedule(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Schedule());
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            Methods.DeleteAllPersons();
            NavigationService.Navigate(new StartPage());
        }

        #region CreateCard

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Data.cardCount >= 20)
                return;

            Data.cardCount++;
            Data.idCreate++;

            // Получаем значения для создания карточки
            string imagePath = GetImagePath((Button)sender);
            string gender = GetGender((Button)sender);
            string age = GetAge();
            string indexForEmploymentRate = listOfOccupationalStatus[0];
            string indexMaritalStatus = listOfFamilySituations[0];
            bool diseaseCheck = false;

            Border card = CreateCard(imagePath, "", gender, age, indexForEmploymentRate, indexMaritalStatus, diseaseCheck);
            Person person = new Person(Data.idCreate, "", age, imagePath, gender, indexForEmploymentRate, indexMaritalStatus, diseaseCheck);
            
            card.Tag = Data.idCreate;
            cardsContainer.Children.Add(card);
        }

        private string GetImagePath(Button button)
        {
            return button.Content.ToString() == Gender.Male.ToString() ? Data.pathUserMan : Data.pathUserWoman;
        }

        private string GetGender(Button button)
        {
            return button.Content.ToString() == Gender.Male.ToString() ? Gender.Male.ToString() : Gender.Female.ToString();
        }

        private string GetAge()
        {
            return isChildChB.IsChecked == true ? Age.Child.ToString() : Age.Adult.ToString();
        }

        public Border CreateCard(string pathURLImage, string name, string gender, string age, string nameForEmploymentRate, string nameMaritalStatus, bool diseaseCheck)
        {
            Border card = new Border
            {
                Style = (Style)FindResource("CustomBorder"),
                Margin = new Thickness(5)
            };

            StackPanel stackPanel = new StackPanel();

            Image userImage = new Image
            {
                Name = "userImage",
                Style = (Style)FindResource("ImageCard"),
                Source = new BitmapImage(new Uri(pathURLImage, UriKind.Relative))
            };

            TextBox userName = new TextBox
            {
                Name = "userName",
                Text = name,
                Margin = new Thickness(0, 0, 0, 15),
                Style = (Style)FindResource("TextBoxCard")
            };

            StackPanel genderTypePanel = new StackPanel
            {
                Style = (Style)FindResource("StackPanelCard")
            };

            TextBlock genderTextBlock = new TextBlock
            {
                Text = "Пол:",
                Style = (Style)FindResource("TextBlockCard")
            };
            Button genderBtn = new Button
            {
                Name = "genderBtn",
                Content = gender,
                Style = (Style)FindResource("ButtonCard"),
                Width = 84
            };
            genderBtn.Click += genderBtn_Click;

            TextBlock typeTextBlock = new TextBlock
            {
                Text = "Тип:",
                Style = (Style)FindResource("TextBlockCard")
            };
            Button ageBtn = new Button
            {
                Name = "ageBtn", 
                Content = age,
                Style = (Style)FindResource("ButtonCard"),
                Width = 84,
                Tag = Data.idCreate
            };
            ageBtn.Click += ageBtn_Click;

            genderTypePanel.Children.Add(genderTextBlock);
            genderTypePanel.Children.Add(genderBtn);
            genderTypePanel.Children.Add(typeTextBlock);
            genderTypePanel.Children.Add(ageBtn);

            StackPanel employmentPanel = new StackPanel
            {
                Style = (Style)FindResource("StackPanelCard")
            };

            TextBlock employmentTextBlock = new TextBlock
            {
                Text = "Степень занятости:",
                Style = (Style)FindResource("TextBlockCard")
            };
            ComboBox employmentRateCmB = new ComboBox
            {
                Name = "employmentRateCmB", 
                Style = (Style)FindResource("ComboBoxCard"),
                ItemsSource = listOfOccupationalStatus,
                SelectedItem = nameForEmploymentRate,
            };

            employmentPanel.Children.Add(employmentTextBlock);
            employmentPanel.Children.Add(employmentRateCmB);

            StackPanel maritalStatusPanel = new StackPanel
            {
                Style = (Style)FindResource("StackPanelCard")
            };

            TextBlock maritalStatusTextBlock = new TextBlock
            {
                Text = "Положение:",
                Style = (Style)FindResource("TextBlockCard")
            };
            ComboBox maritalStatusCmB = new ComboBox
            {
                Name = "maritalStatusCmB",
                Style = (Style)FindResource("ComboBoxCard"),
                ItemsSource = listOfFamilySituations,
                SelectedItem = nameMaritalStatus,
            };

            maritalStatusPanel.Children.Add(maritalStatusTextBlock);
            maritalStatusPanel.Children.Add(maritalStatusCmB);

            StackPanel diseasePanel = new StackPanel
            {
                Margin = new Thickness(5, 5, 0, 15),
                Style = (Style)FindResource("StackPanelCard")
            };

            TextBlock diseaseTextBlock = new TextBlock
            {
                Text = "Болеет",
                Style = (Style)FindResource("TextBlockCard")
            };

            CheckBox diseaseChB = new CheckBox
            {
                Name = "diseaseChB",
                Style = (Style)FindResource("CustomCheckBox"),
                IsChecked = diseaseCheck
            };

            diseasePanel.Children.Add(diseaseTextBlock);
            diseasePanel.Children.Add(diseaseChB);

            Button userDeleteBtn = new Button
            {
                Name = "userDeleteBtn",
                Content = "Удалить",
                Style = (Style)FindResource("ButtonCard")
            };
            userDeleteBtn.Click += userDeleteBtn_Click;

            stackPanel.Children.Add(userImage);
            stackPanel.Children.Add(userName);
            stackPanel.Children.Add(genderTypePanel);
            stackPanel.Children.Add(employmentPanel);
            stackPanel.Children.Add(maritalStatusPanel);
            stackPanel.Children.Add(diseasePanel);
            stackPanel.Children.Add(userDeleteBtn);

            card.Child = stackPanel;

            card.MouseEnter += Card_MouseEnter;
            card.MouseLeave += DataUpdate_MouseLeave;

            return card;
        }
        
        #endregion

        private void genderBtn_Click(object sender, RoutedEventArgs e)
        {
            Button genderButton = sender as Button;
            if (genderButton != null)
            {
                bool check = genderButton.Content.ToString() == Gender.Male.ToString() ? true : false;
                ChangeCardById(_tagId, check ? Data.pathUserWoman : Data.pathUserMan);
                genderButton.Content = check ? Gender.Female.ToString() : Gender.Male.ToString();
            }
        }

        private void ageBtn_Click(object sender, RoutedEventArgs e)
        {
            Button ageButton = sender as Button;
            if (ageButton != null)
                ageButton.Content = ageButton.Content.ToString() == Age.Adult.ToString() ? Age.Child.ToString() : Age.Adult.ToString();
        }

        private void DataUpdate_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TryGetPersonData(out Person person)) { }
        }

        #region DataUpdate

        private bool TryGetPersonData(out Person person)
        {
            person = null;

            foreach (var child in cardsContainer.Children)
            {
                if (child is Border border && border.Tag != null && (int)border.Tag == _tagId)
                {
                    // Найти дочерние элементы
                    if (border.Child is StackPanel stackPanel)
                    {
                        string pathURLImage = GetImageSource(stackPanel);
                        string name = GetTextBoxValue(stackPanel);
                        string gender = GetButtonContent(stackPanel, 1);
                        string age = GetButtonContent(stackPanel, 3);
                        string indexForEmploymentRate = GetComboBoxSelectedIndex(stackPanel, 3);
                        string indexMaritalStatus = GetComboBoxSelectedIndex(stackPanel, 4);
                        bool diseaseCheck = GetCheckBoxValue(stackPanel, 5);

                        person = new Person(_tagId, name, age, pathURLImage, gender, indexForEmploymentRate, indexMaritalStatus, diseaseCheck);
                        return true;
                    }
                }
            }

            return false;
        }

        private string GetImageSource(StackPanel stackPanel)
        {
            if (stackPanel.Children.Count > 0 && stackPanel.Children[0] is Image image)
            {
                // Проверяем, что источник изображения - это BitmapImage
                if (image.Source is BitmapImage bitmapImage)
                {
                    // Получаем URI изображения
                    Uri uri = bitmapImage.UriSource;
                    if (uri != null)
                    {
                        // Возвращаем относительный путь
                        return uri.ToString();
                    }
                }
            }
            return string.Empty;
        }

        private string GetTextBoxValue(StackPanel stackPanel)
        {
            if (stackPanel.Children[1] is TextBox textBox)
            {
                return textBox.Text;
            }
            return string.Empty;
        }

        private string GetButtonContent(StackPanel stackPanel, int buttonIndex)
        {
            if (stackPanel.Children[2] is StackPanel stackBtn && stackBtn.Children[buttonIndex] is Button button)
            {
                return button.Content.ToString();
            }
            return string.Empty;
        }

        private string GetComboBoxSelectedIndex(StackPanel stackPanel, int comboBoxIndex)
        {
            if (stackPanel.Children[comboBoxIndex] is StackPanel stackCombo && stackCombo.Children[1] is ComboBox comboBox)
            {
                return comboBox.SelectedItem.ToString();
            }
            return "Error";
        }

        private bool GetCheckBoxValue(StackPanel stackPanel, int checkBoxIndex)
        {
            if (stackPanel.Children[checkBoxIndex] is StackPanel stackChB && stackChB.Children[1] is CheckBox checkBox)
            {
                return checkBox.IsChecked ?? false;
            }
            return false;
        }

        #endregion

        // Обработчик события наведения на Border
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                int cardId = (int)border.Tag;
                _tagId = cardId;
            }
        }

        // Метод для изменения дочерних элементов по Tag
        private void ChangeCardById(int cardId, string newURL)
        {
            foreach (var child in cardsContainer.Children)
            {
                if (child is Border border && border.Tag != null && (int)border.Tag == cardId)
                {
                    // Найти дочерние элементы
                    if (border.Child is StackPanel stackPanel)
                    {
                        if (stackPanel.Children[0] is Image image)
                        {
                            // Меняем источник изображения
                            image.Source = new BitmapImage(new Uri(newURL, UriKind.Relative));
                        }
                    }
                    return;
                }
            }
        }

        private void userDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                // Находим родительский StackPanel
                StackPanel cardStackPanel = (StackPanel)deleteButton.Parent;
                // Находим родительский Border
                Border card = (Border)cardStackPanel.Parent;
                // Удаляем карточку из контейнера
                cardsContainer.Children.Remove(card);

                Person.RemovePerson(_tagId);
                Data.cardCount--;
            }
        }

        private void PersonInfo_Click(object sender, RoutedEventArgs e)
        {
            Methods.MessageAllPersons();
        }

        private void SwitchingMode_CheckedChanged(object sender, EventArgs e)
        {
            Data.adminMode = (bool)((CheckBox)sender).IsChecked;
            DefaultParams();
        }
    }
}
