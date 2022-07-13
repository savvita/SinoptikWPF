using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SinoptikWPF.Model
{
    public class SinoptikPageModel : INotifyPropertyChanged
    {

        private TodayInfo? todayInfo;
        public TodayInfo? TodayInfo
        {
            get => todayInfo;
            set
            {
                todayInfo = value;
                OnPropertyChanged(nameof(TodayInfo));
            }
        }

        private string? sunDay;
        public string? SunDay
        {
            get => sunDay;
            set
            {
                sunDay = value;
                OnPropertyChanged(nameof(SunDay));
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

        private string? currentTemp;
        public string? CurrentTemp
        {
            get => currentTemp;
            set
            {
                currentTemp = value;
                OnPropertyChanged(nameof(CurrentTemp));
            }
        }

        private string? todayTimeTemperature;
        public string? TodayTimeTemperature
        {
            get => todayTimeTemperature;
            set
            {
                todayTimeTemperature = value;
                OnPropertyChanged(nameof(TodayTimeTemperature));
            }
        }

        private HistoricalTemperature? minTemperature;
        public HistoricalTemperature? MinTemperature
        {
            get => minTemperature;
            set
            {
                minTemperature = value;
                OnPropertyChanged(nameof(MinTemperature));
            }
        }

        private HistoricalTemperature? maxTemperature;
        public HistoricalTemperature? MaxTemperature
        {
            get => maxTemperature;
            set
            {
                maxTemperature = value;
                OnPropertyChanged(nameof(MaxTemperature));
            }
        }

        private string? lastYears;
        public string? LastYears
        {
            get => lastYears;
            set
            {
                lastYears = value;
                OnPropertyChanged(nameof(LastYears));
            }
        }

        public ObservableCollection<HourTemperatureModel> HoursTemp { get; set; }

        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string? signs;
        public string? Signs
        {
            get => signs;
            set
            {
                signs = value;
                OnPropertyChanged(nameof(Signs));
            }
        }

        public SinoptikPageModel()
        {
            HoursTemp = new ObservableCollection<HourTemperatureModel>();
            MinTemperature = new HistoricalTemperature();
            MaxTemperature = new HistoricalTemperature();
            TodayInfo = new TodayInfo();

            for (int i = 0; i < GlobalSettings.Hours; i++)
            {
                HoursTemp.Add(new HourTemperatureModel());
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
