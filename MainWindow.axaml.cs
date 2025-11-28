using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Linq; 

namespace AvaloniaApp
{
    public partial class MainWindow : Window
    {
        private string _surname = "";
        private string _name = "";
        private string _patronymic = "";
        private string _phone = "";
        private int _age = 18;
        private bool _hasConsent = false;

        public MainWindow()
        {
            InitializeComponent();
            
            // Olayları bağlıyoruz
            var noPatronymicCheck = this.FindControl<CheckBox>("NoPatronymicCheck");
            if (noPatronymicCheck != null) noPatronymicCheck.IsCheckedChanged += NoPatronymicCheck_Changed;

            var phoneBox = this.FindControl<TextBox>("PhoneBox");
            if (phoneBox != null) phoneBox.TextChanged += PhoneBox_TextChanged;

            var ageSlider = this.FindControl<Slider>("AgeSlider");
            if (ageSlider != null) ageSlider.ValueChanged += AgeSlider_ValueChanged;

            var consentSwitch = this.FindControl<CheckBox>("ConsentSwitch");
            if (consentSwitch != null) consentSwitch.IsCheckedChanged += ConsentSwitch_Changed;

            var submitButton = this.FindControl<Button>("SubmitButton");
            if (submitButton != null) submitButton.Click += SubmitButton_Click;
        }

        private void NoPatronymicCheck_Changed(object? sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var patronymicBox = this.FindControl<TextBox>("PatronymicBox");

            if (checkBox != null && patronymicBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;

                if (isChecked)
                {
                    patronymicBox.IsEnabled = false;
                    patronymicBox.Text = "Не указано"; // Belirtilmedi (Rusça)
                    _patronymic = "Не указано";
                }
                else
                {
                    patronymicBox.IsEnabled = true;
                    patronymicBox.Text = "";
                    _patronymic = "";
                }
            }
        }

        private void PhoneBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            var phoneBox = sender as TextBox;
            var errorText = this.FindControl<TextBlock>("PhoneErrorText");

            if (phoneBox != null && errorText != null)
            {
                string input = phoneBox.Text ?? "";

                if (string.IsNullOrEmpty(input))
                {
                    errorText.IsVisible = false;
                    _phone = "";
                    return;
                }

                bool isNumeric = input.All(char.IsDigit);

                if (!isNumeric)
                {
                    errorText.Text = "Пожалуйста, вводите только цифры!"; // Sadece sayı giriniz
                    errorText.IsVisible = true;
                    phoneBox.Foreground = Brushes.Red; 
                }
                else
                {
                    errorText.IsVisible = false; 
                    phoneBox.Foreground = Brushes.White; 
                    _phone = input;
                }
            }
        }

        private void AgeSlider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var label = this.FindControl<TextBlock>("AgeLabel");
            if (label != null)
            {
                _age = (int)e.NewValue;
                label.Text = _age.ToString();
            }
        }

        private void ConsentSwitch_Changed(object? sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var button = this.FindControl<Button>("SubmitButton");

            if (checkBox != null && button != null)
            {
                _hasConsent = checkBox.IsChecked ?? false;
                button.IsEnabled = _hasConsent; 
            }
        }

        private async void SubmitButton_Click(object? sender, RoutedEventArgs e)
        {
            _name = this.FindControl<TextBox>("NameBox")?.Text ?? "";
            _surname = this.FindControl<TextBox>("SurnameBox")?.Text ?? "";
            if(this.FindControl<TextBox>("PatronymicBox")?.IsEnabled == true)
            {
                 _patronymic = this.FindControl<TextBox>("PatronymicBox")?.Text ?? "";
            }

            string gender = "Не указано";
            if (this.FindControl<RadioButton>("MaleRadio")?.IsChecked == true) gender = "Мужской";
            if (this.FindControl<RadioButton>("FemaleRadio")?.IsChecked == true) gender = "Женский";
            if (this.FindControl<RadioButton>("OtherRadio")?.IsChecked == true) gender = "Другое";

            // Sonuç Mesajı
            var msg = $"РЕГИСТРАЦИЯ УСПЕШНА!\n\nИмя: {_name}\nФамилия: {_surname}\nОтчество: {_patronymic}\nВозраст: {_age}\nТелефон: {_phone}\nПол: {gender}";

            var dlg = new Window
            {
                Title = "Результат",
                Width = 300,
                Height = 300, // Mesaj sığsın diye biraz uzattım
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Thickness(20),
                    Spacing = 10,
                    Children =
                    {
                        new TextBlock { Text = msg, TextWrapping = TextWrapping.Wrap },
                        new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Thickness(0,20,0,0) }
                    }
                }
            };
            
            var okButton = (dlg.Content as StackPanel)?.Children[1] as Button;
            if(okButton != null) okButton.Click += (_, _) => dlg.Close();

            await dlg.ShowDialog(this);
        }
    }
}