using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvaloniaApp.Models
{
    public class FormData : INotifyPropertyChanged
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private int _age;
        private string _phone = string.Empty;
        private string _gender = string.Empty;
        private bool _hasConsent;

        public string Surname { get => _surname; set => SetProperty(ref _surname, value); }
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Patronymic { get => _patronymic; set => SetProperty(ref _patronymic, value); }
        public int Age { get => _age; set => SetProperty(ref _age, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
        public string Gender { get => _gender; set => SetProperty(ref _gender, value); }
        public bool HasConsent { get => _hasConsent; set => SetProperty(ref _hasConsent, value); }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
            backingStore = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}