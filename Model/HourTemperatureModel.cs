using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SinoptikWPF.Model
{
    public class HourTemperatureModel : INotifyPropertyChanged
    {
        private string? title;
        public string? Title
        {
            get => title;

            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string? time;
        public string? Time
        {
            get => time;

            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private string? weatherImage;
        public string? WeatherImage
        {
            get => weatherImage;

            set
            {
                weatherImage = value;
                OnPropertyChanged(nameof(WeatherImage));
            }
        }

        private string? weatherImageTitle;
        public string? WeatherImageTitle
        {
            get => weatherImageTitle;

            set
            {
                weatherImageTitle = value;
                OnPropertyChanged(nameof(WeatherImageTitle));
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

        private string? temperatureFeelLike;
        public string? TemperatureFileLike
        {
            get => temperatureFeelLike;
            set
            {
                temperatureFeelLike = value;
                OnPropertyChanged(nameof(TemperatureFileLike));
            }
        }

        private string? pressure;
        public string? Pressure
        {
            get => pressure;
            set
            {
                pressure = value;
                OnPropertyChanged(nameof(Pressure));
            }
        }

        private string? humidity;
        public string? Humidity
        {
            get => humidity;
            set
            {
                humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        private string? wind;
        public string? Wind
        {
            get => wind;
            set
            {
                wind = value;
                OnPropertyChanged(nameof(Wind));
            }
        }


        private string? windDirection;
        public string? WindDirection
        {
            get => windDirection;
            set
            {
                windDirection = value;
                OnPropertyChanged(nameof(WindDirection));
            }
        }

        private string? precipitation;
        public string? Precipitation
        {
            get => precipitation;
            set
            {
                precipitation = value;
                OnPropertyChanged(nameof(Precipitation));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
