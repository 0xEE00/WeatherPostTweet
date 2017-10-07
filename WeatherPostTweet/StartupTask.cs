using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Xml;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Windows.UI.Popups;
using Tweetinvi;
using Windows.Foundation;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace WeatherPostTweet
{
    public sealed class StartupTask : IBackgroundTask
    {
        string weatherInfo = "";
        char delimetar = ',';
        char delimetar2 = ':';


        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral(); //Sprečava zatvaranje glavnog procesa dok se u pozadini izvršavaju asinhroni procesi
            while (true)
            {
                weatherInfo = await MakeWebRequest(@"http://www.hidmet.gov.rs/ciril/osmotreni/kosutnjak.xml");
                weatherInfo = GetWeatherInfoXML(weatherInfo);
                //var tweets = Search.SearchTweets("Датум осматрања:");
                Auth.SetUserCredentials("MuEtY8o5bgVF2kbX5RcXDp6rA", "Xw2qiJlj8qeB2pv2W7pe45Y55D9IXPrLShQb6TA8T4ZbhmHpYs", "61542070-19Fwt5IhgVeQufhBiqX7Hi9AGnsoeKLZnos4Fv4Nj", "bNjYwTVwgetZXgo6cZ5jE6tjzsh0XOCa7LLOz661hIkzj");
                ITweet publishedTweet = Tweet.PublishTweet(SplitAndFormat(weatherInfo));
                //var tweetId = publishedTweet.Id.ToString();
                await Task.Delay(600000);//60000 = 1min; 360000 = 1h
                publishedTweet.Destroy();
            }
            deferral.Complete();
        }

        private string SplitAndFormat(string weatherInfo)
        {
            String[] substring = weatherInfo.Split(delimetar);

            String[] substringDate = substring[0].Split(delimetar2);
            String[] substringTime = substring[2].Split(delimetar2);
            String[] substringTemperature = substring[3].Split(delimetar2);
            String[] substringPressure = substring[4].Split(delimetar2);
            String[] substringHumidity = substring[5].Split(delimetar2);

            string formatWeatherInfo = "#Belgrade" + "\n" +
                    "Date: " + substringDate[1].Trim() + "\n" +
                    "Time: " + substringTime[1].Trim() + ":" + substringTime[2].Trim() + ":" + substringTime[3].Trim() + "\n" +
                    "Temperature: " + substringTemperature[1].Trim() + "\n" +
                    "Pressure: " + substringPressure[1].Trim() + "\n" +
                    "Humidity: " + substringHumidity[1].Trim() + "\n" +
                    "#RaspberryPi";
            return formatWeatherInfo;
        }

        private string GetWeatherInfoXML(string weatherInfo)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(weatherInfo);
            XmlNodeList xmlNodes = xml.GetElementsByTagName("summary");
            return xmlNodes[0].InnerText;
        }

        public IAsyncOperation<string> MakeWebRequest(string uri) //IAsyncOperation je validna Windows Runtime povratna vrednost koja predstavlja jednu asinhronu operaciju
        {

            return this.GetUriContentAsynHelper(uri).AsAsyncOperation();
            //var dialog = new MessageDialog(weatherInfo);
            //await dialog.ShowAsync();
        }



        private async Task<string> GetUriContentAsynHelper(string uri) //S obzirom da je UWP Windows Runtime ne možemo da imamo javnu metodu koja vraća Task
        {
            //Initialize WebClient
            HttpClient httpClient = new HttpClient();
            //Call service
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(uri));
            //Read response
            string responseText = await response.Content.ReadAsStringAsync();
            //Return result
            return responseText;


            //Može i ovako
            /*
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);

            return content;
            */
        }
    }
}
