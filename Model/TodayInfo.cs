using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SinoptikWPF.Model
{
    public class TodayInfo : INotifyPropertyChanged
    {
        private string? dayOfWeek;
        public string? DayOfWeek
        {
            get => dayOfWeek;
            set
            {
                dayOfWeek = value;
                OnPropertyChanged(nameof(DayOfWeek));
            }
        }

        private string? day;
        public string? Day
        {
            get => day;
            set
            {
                day = value;
                OnPropertyChanged(nameof(Day));
            }
        }

        private string? month;
        public string? Month
        {
            get => month;
            set
            {
                month = value;
                OnPropertyChanged(nameof(Month));
            }
        }

        private string? dayWeatherImage;
        public string? DayWeatherImage
        {
            get => dayWeatherImage;
            set
            {
                dayWeatherImage = value;
                OnPropertyChanged(nameof(DayWeatherImage));
            }
        }

        private string? minTemperatureText;
        public string? MinTemperatureText
        {
            get => minTemperatureText;
            set
            {
                minTemperatureText = value;
                OnPropertyChanged(nameof(MinTemperatureText));
            }
        }

        private string? maxTemperatureText;
        public string? MaxTemperatureText
        {
            get => maxTemperatureText;
            set
            {
                maxTemperatureText = value;
                OnPropertyChanged(nameof(MaxTemperatureText));
            }
        }

        private string? minTemperature;
        public string? MinTemperature
        {
            get => minTemperature;
            set
            {
                minTemperature = value;
                OnPropertyChanged(nameof(MinTemperature));
            }
        }

        private string? maxTemperature;
        public string? MaxTemperature
        {
            get => maxTemperature;
            set
            {
                maxTemperature = value;
                OnPropertyChanged(nameof(MaxTemperature));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
