using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SinoptikWPF.Model;

namespace SinoptikWPF.ViewModel
{
    internal class SinoptikViewModel : INotifyPropertyChanged
    {
        public SinoptikPageModel SinoptikPage { get; private set; } = new SinoptikPageModel();

        private HttpClient httpClient = new HttpClient();

        public SinoptikViewModel()
        {
            if (!Directory.Exists(GlobalSettings.FilesFolder))
            {
                Directory.CreateDirectory(GlobalSettings.FilesFolder);
            }

            SetActualInfo();

            Thread thread = new Thread(SetActualInfo)
            {
                IsBackground = true
            };
            thread.Start();

        }

        private async void SetActualInfo()
        {
            while (true)
            {
                try
                {
                    string html = await httpClient.GetStringAsync(GlobalSettings.URL);

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    SetWeatherImage(htmlDoc);
                    SetCurrentTemperature(htmlDoc);
                    SetTodayTimeWeather(htmlDoc);
                    SetHistoricalTemperature(htmlDoc);

                    SetSunDayValue(htmlDoc);

                    for (int i = 0; i < SinoptikPage.HoursTemp.Count; i++)
                    {
                        SetHoursValues(htmlDoc, i);
                    }

                    SetDescription(htmlDoc);
                    SetSigns(htmlDoc);
                    SetTodayInfo(htmlDoc);
                }
                catch { }
                Thread.Sleep(10000);
            }
        }

        private void SetCurrentTemperature(HtmlDocument doc)
        {
            HtmlNode? node = doc.DocumentNode.SelectSingleNode("//p[@class='today-temp']");

            if (node != null)
            {
                SinoptikPage.CurrentTemp = GetTemperatureString(node.InnerText);
            }
        }

        private void SetTodayTimeWeather(HtmlDocument doc)
        {
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//p[@class='today-time']");

            if (node != null)
            {
                SinoptikPage.TodayTimeTemperature = node.InnerHtml;
            }
        }

        private void SetSunDayValue(HtmlDocument doc)
        {
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='infoDaylight']");

            if (node != null)
            {
                SinoptikPage.SunDay = node.InnerText;
            }
        }

        private async void SetWeatherImage(HtmlDocument doc)
        {
            HtmlNode? node = doc.DocumentNode.SelectSingleNode("//div[@class='img']//img");

            SinoptikPage.WeatherImage = await GetImageFromNode(node);
        }

        public async void SetHoursValues(HtmlDocument doc, int index)
        {
            //Title
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='titles']//p");

            if (nodes != null)
            {
                if (index > 1)
                {
                    SinoptikPage.HoursTemp[index].Title = nodes[index - 2].InnerText;
                }
            }

            //Time
            nodes = doc.DocumentNode.SelectNodes("//tr[@class='gray time']//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].Time = nodes[index].InnerHtml;
            }

            //WeatherImage & WeatherTitle
            nodes = doc.DocumentNode.SelectNodes("//tr[@class='img weatherIcoS']//td");

            if (nodes != null)
            {
                HtmlAttribute? attr = nodes[index].ChildNodes[1].Attributes.Where(x => x.Name.Equals("title")).FirstOrDefault();

                if (attr != null)
                {
                    SinoptikPage.HoursTemp[index].WeatherImageTitle = attr.Value;
                }

                SinoptikPage.HoursTemp[index].WeatherImage = await GetImageFromNode(nodes[index].ChildNodes[1].FirstChild);
            }

            //Temperature
            nodes = doc.DocumentNode.SelectNodes("//tr[@class='temperature']//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].Temperature = GetTemperatureString(nodes[index].InnerHtml);
            }

            //TemperatureFileLike
            nodes = doc.DocumentNode.SelectNodes("//tr[@class='temperatureSens']//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].TemperatureFileLike = GetTemperatureString(nodes[index].InnerHtml);
            }

            //Humidity
            nodes = doc.DocumentNode.SelectNodes("//table[@class='weatherDetails']//tbody//tr[6]//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].Humidity = nodes[index].InnerHtml;
            }

            //Wind & WindDirection
            nodes = doc.DocumentNode.SelectNodes("//tr[@class='gray']//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].Pressure = nodes[index].InnerHtml;

                HtmlAttribute? toolTip = nodes[GlobalSettings.Hours + index].ChildNodes[1].Attributes.Where(x => x.Name.Equals("data-tooltip")).FirstOrDefault();

                if (toolTip != null)
                {
                    SinoptikPage.HoursTemp[index].WindDirection = toolTip.Value;
                }

                SinoptikPage.HoursTemp[index].Wind = nodes[GlobalSettings.Hours + index].ChildNodes[1].InnerHtml;
            }

            //Precipitation
            nodes = doc.DocumentNode.SelectNodes("//table[@class='weatherDetails']//tbody//tr[8]//td");

            if (nodes != null)
            {
                SinoptikPage.HoursTemp[index].Precipitation = nodes[index].InnerHtml;
            }
        }


        private string GetTemperatureString(string input)
        {
            int idx = input.IndexOf(input.First(x => !Char.IsDigit(x) && x != '-' && x != '+'));
            return input[0..idx] + "\u00B0C";
        }

        private async Task<string?> GetImageFromNode(HtmlNode? node)
        {
            if(node == null)
            {
                return null;
            }

            HtmlAttribute? attr = node.Attributes.First(x => x.Name.Equals("src"));
            
            if (attr != null)
            {
                string? uri = $"https:{attr.Value}";

                try
                {
                    byte[] bytes = await httpClient.GetByteArrayAsync(uri);

                    string fileFullName = Path.Combine(GlobalSettings.FilesFolder, Path.GetFileName(uri));

                    using (FileStream file = new FileStream(fileFullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        file.Write(bytes, 0, bytes.Count());
                    }

                    return fileFullName;
                }
                catch { }
            }

            return null;
        }

        private void SetHistoricalTemperature(HtmlDocument doc)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//p[@class='infoHistoryval']//span");

            if (nodes != null && nodes.Count > 1)
            {
                if (SinoptikPage.MaxTemperature != null)
                {
                    SinoptikPage.MaxTemperature.Temperature = GetTemperatureString(nodes[0].InnerHtml);
                }

                if (SinoptikPage.MinTemperature != null)
                {
                    SinoptikPage.MinTemperature.Temperature = GetTemperatureString(nodes[1].InnerHtml);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//p[@class='infoHistoryval']");

            if (nodes != null && nodes.Count > 0)
            {
                string info = nodes[0].InnerHtml;
                if (SinoptikPage.MaxTemperature != null)
                {
                    SinoptikPage.MaxTemperature.Year = info.Substring(info.IndexOf('(') + 1, 4);
                }

                if (SinoptikPage.MinTemperature != null)
                {
                    SinoptikPage.MinTemperature.Year = info.Substring(info.LastIndexOf('(') + 1, 4);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//p[@class='infoHistoryval']//i");

            if (nodes != null && nodes.Count > 1)
            {
                if (SinoptikPage.MaxTemperature != null)
                {
                    SinoptikPage.MaxTemperature.Text = nodes[0].InnerHtml;
                }

                if (SinoptikPage.MinTemperature != null)
                {
                    SinoptikPage.MinTemperature.Text = nodes[1].InnerHtml;
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//p[@class='infoHistory']");

            if (nodes != null)
            {
                SinoptikPage.LastYears = nodes[0].InnerText;
            }
        }

        private void SetDescription(HtmlDocument doc)
        {
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='description']");

            if (node != null)
            {
                SinoptikPage.Description = node.InnerText;
            }
        }

        private void SetSigns(HtmlDocument doc)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='description']");

            if (nodes != null)
            {
                SinoptikPage.Signs = nodes[1].InnerText;
            }
        }

        private async void SetTodayInfo(HtmlDocument doc)
        {
            if(SinoptikPage.TodayInfo == null)
            {
                return;
            }

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//p[@class='day-link']");

            if (nodes != null)
            {
                SinoptikPage.TodayInfo.DayOfWeek = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("//p[@class='date ']");

            if (nodes != null)
            {
                SinoptikPage.TodayInfo.Day = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("//p[@class='month']");

            if (nodes != null)
            {
                SinoptikPage.TodayInfo.Month = nodes[0].InnerText;
            }

            nodes = doc.DocumentNode.SelectNodes("//div[@id='bd1']//div[1]//img");

            if (nodes != null)
            {
                SinoptikPage.TodayInfo.DayWeatherImage = await GetImageFromNode(nodes[0]);
            }


            nodes = doc.DocumentNode.SelectNodes("//div[@id='bd1']//div[2]//div");

            if (nodes != null)
            {
                string[] cols = nodes[0].InnerText.Split(' ');
                SinoptikPage.TodayInfo.MinTemperatureText = cols[0];
                SinoptikPage.TodayInfo.MinTemperature = GetTemperatureString(cols[1]);

                cols = nodes[1].InnerText.Split(' ');
                SinoptikPage.TodayInfo.MaxTemperatureText = cols[0];
                SinoptikPage.TodayInfo.MaxTemperature = GetTemperatureString(cols[1]);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
