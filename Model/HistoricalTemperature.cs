using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SinoptikWPF.Model
{
    public class HistoricalTemperature : INotifyPropertyChanged
    {
        private string? year;
        public string? Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private string? temperature;
        public string? Temperature
        {
            get => temperature;
            set
            {
                temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private string? text;
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
